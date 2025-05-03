using BuildingBlocks.Behaviors;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application
{
	public static class DependencyInjection
	{
		public static IServiceCollection AddApplicationServices(this IServiceCollection services)
		{
			services.AddMediatR(ops =>
				{
				ops.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly());
					ops.AddOpenBehavior(typeof(ValidationBehavior<,>));
					ops.AddOpenBehavior(typeof(LoggingBehavior<,>));
				});

			//Register services here
			return services;
		}
	}
}
