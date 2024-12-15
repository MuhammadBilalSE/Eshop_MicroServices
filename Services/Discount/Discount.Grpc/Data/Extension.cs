using Microsoft.EntityFrameworkCore;

namespace Discount.Grpc.Data
{
	public static class Extension
	{
		public static IApplicationBuilder UseMigration(this IApplicationBuilder app)
		{
			using var scope = app.ApplicationServices.CreateScope();
			using var discountcontext = scope.ServiceProvider.GetRequiredService<DiscountContext>();
			discountcontext.Database.MigrateAsync();	
			return app;
		}
	}
}
