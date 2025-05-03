using BuildingBlocks.Exceptions.Handler;
using Carter;

namespace Ordering.API
{
	public static class DependencyInjection
	{
		public static IServiceCollection AddApiServices(this IServiceCollection services)
		{
			//Register services here
			services.AddCarter();
			services.AddExceptionHandler<CustomExceptionHandler>();
		    services.AddHealthChecks();
			//services.

			return services;
		}

		public static WebApplication UseApiServices(this WebApplication app)
		{
			app.MapCarter();
			app.UseExceptionHandler(ops => { });
			app.UseHealthChecks("/health");
			return app;
		}
	}
}
