using BuildingBlocks.Behaviors;
using BuildingBlocks.Exceptions.Handler;
using Catalog.API.Data;
using FluentValidation;
using HealthChecks.UI.Client;

var builder = WebApplication.CreateBuilder(args); //CreateBuilder(args);

// learn more about configuring swagger/openapi at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCarter();
builder.Services.AddMediatR(
	config =>
	{
		config.RegisterServicesFromAssemblies(typeof(Program).Assembly);
		config.AddOpenBehavior(typeof(ValidationBehavior<,>));
		config.AddOpenBehavior(typeof(LoggingBehavior<,>));
	});
builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);
builder.Services.AddMarten(op =>
{
	op.Connection(builder.Configuration.GetConnectionString("datacenter")!);
}).UseLightweightSessions();
if (builder.Environment.IsDevelopment())
	builder.Services.InitializeMartenWith<CatalogInitialData>();
	builder.Services.AddExceptionHandler<CustomExceptionHandler>();
	builder.Services.AddHealthChecks().AddNpgSql(builder.Configuration.GetConnectionString("datacenter"));

var app = builder.Build();

// configure the http request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapCarter();
app.UseExceptionHandler(options => { });
app.UseHealthChecks("/health",
	new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
	{
		ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
	});
app.Run();

