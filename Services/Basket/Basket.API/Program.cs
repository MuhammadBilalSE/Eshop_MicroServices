using Basket.API.Data;
using Basket.API.Models;
using BuildingBlocks.Behaviors;
using BuildingBlocks.Exceptions.Handler;
using Carter;
using Discount.Grpc.Protos;
using HealthChecks.UI.Client;
using Marten;
using Grpc;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);
//Add Application serivces to container
builder.Services.AddCarter();
builder.Services.AddMediatR(
	config => {
	config.RegisterServicesFromAssembly(typeof(Program).Assembly);
		config.AddOpenBehavior(typeof(ValidationBehavior<,>));
		config.AddOpenBehavior(typeof(LoggingBehavior<,>));
	});
// Add data Services
builder.Services.AddMarten(o =>
{
	o.Connection(builder.Configuration.GetConnectionString("DataCenter")!);
	o.Schema.For<ShoppingCart>().Identity(x => x.UserName);
}).UseLightweightSessions();
builder.Services.AddScoped<IBasketRepository,BasketRepository>();
builder.Services.Decorate<IBasketRepository, CachedBasketRepository>();

builder.Services.AddStackExchangeRedisCache(
	o =>{
		o.Configuration = builder.Configuration.GetConnectionString("Redis");
	});

builder.Services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>(
	options =>
	{
		options.Address = new Uri(builder.Configuration["GrpcSettings:DiscountUrl"]!);
	})
	.ConfigurePrimaryHttpMessageHandler(
	() =>
	{
		return new HttpClientHandler
		{
			ServerCertificateCustomValidationCallback =
			HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
		};
	});

builder.Services.AddExceptionHandler<CustomExceptionHandler>();
builder.Services.AddHealthChecks()
	.AddNpgSql(builder.Configuration.GetConnectionString("DataCenter")!)
	.AddRedis(builder.Configuration.GetConnectionString("Redis")!);

var app = builder.Build();
app.MapCarter();
app.UseExceptionHandler(options => { });
app.UseHealthChecks("/health",
	new HealthCheckOptions
	{
		ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
	});
// Configure Https Pipleline


app.Run();
