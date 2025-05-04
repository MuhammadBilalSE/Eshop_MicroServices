using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BuildingBlock.Messaging.MassTransit
{
	public static class Extensions
	{
		public static IServiceCollection AddMessageBraker(this IServiceCollection services, IConfiguration configuration, Assembly? assembly = null)
		{
			services.AddMassTransit(
				config =>
				{
					if (assembly !=null)
					{
						config.AddConsumers(assembly);
					}
					config.SetKebabCaseEndpointNameFormatter();
					config.UsingRabbitMq((context,configurate) =>
					{
						configurate.Host(new Uri(configuration["MessageBroker:Host"]!), 
						host =>
						{
							host.Username(configuration["MessageBroker:UserName"]);
							host.Password(configuration["MessageBroker:Password"]);
						});
						configurate.ConfigureEndpoints(context);
					});
				});

			return services;
		}
	}
}
