using Microsoft.EntityFrameworkCore;
using Ordering.Application.Data;
using Ordering.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Infrastructure.Data
{
	public class ApplicationDBContext : DbContext, IApplicationDBContext
	{
		public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
		{

		}
		public DbSet<Customer> Customers => Set<Customer>();
		public DbSet<Order> Orders =>		Set<Order>();
		public DbSet<Product> Products =>	Set<Product>();
		public DbSet<OrderItem> OrderItems => Set<OrderItem>();


		protected override void OnModelCreating(ModelBuilder builder)
		{
			builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
			base.OnModelCreating(builder);
		}

	}
}
