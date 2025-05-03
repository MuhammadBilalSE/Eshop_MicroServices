using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Infrastructure.Data.Extensions
{
	public static class DbExtension
	{
		public static async Task InitializeDb( this WebApplication web)
		{
			using var scope = web.Services.CreateScope();
			var context = scope.ServiceProvider.GetRequiredService<ApplicationDBContext>();
			context.Database.MigrateAsync().GetAwaiter().GetResult();
			await SeedAsync(context);
		}

		private static async Task SeedAsync(ApplicationDBContext context)
		{
			await SeedCustomerAsync(context);
			await SeedProductAsync(context);
			await SeedOrdersWithItemsAsync(context);
		}

		private static async Task SeedCustomerAsync(ApplicationDBContext context)
		{
			if (!await context.Customers.AnyAsync())
			{
				await context.Customers.AddRangeAsync(InitialData.Customers);
				await context.SaveChangesAsync();
			}
		}

		private static async Task SeedProductAsync(ApplicationDBContext context)
		{
			if (!await context.Products.AnyAsync())
			{
				await context.Products.AddRangeAsync(InitialData.Products);
				await context.SaveChangesAsync();
			}
		}

		private static async Task SeedOrdersWithItemsAsync(ApplicationDBContext context)
		{
			if (!await context.Orders.AnyAsync())
			{
				await context.Orders.AddRangeAsync(InitialData.OrdersWithItems);
				await context.SaveChangesAsync();
			}
		}
	}
}
