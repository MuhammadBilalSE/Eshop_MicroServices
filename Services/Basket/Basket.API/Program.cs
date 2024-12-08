using Basket.API.Models;
using BuildingBlocks.Behaviors;
using Carter;
using Marten;

var builder = WebApplication.CreateBuilder(args);
//Add serivces to container
builder.Services.AddCarter();
builder.Services.AddMediatR(
	config => {
	config.RegisterServicesFromAssembly(typeof(Program).Assembly);
		config.AddOpenBehavior(typeof(ValidationBehavior<,>));
		config.AddOpenBehavior(typeof(LoggingBehavior<,>));
	});
builder.Services.AddMarten(o =>
{
	o.Connection(builder.Configuration.GetConnectionString("DataCenter")!);
	o.Schema.For<ShoppingCart>().Identity(x => x.UserName);
}).UseLightweightSessions();


var app = builder.Build();
app.MapCarter();
// Configure Https Pipleline


app.Run();
