using Microsoft.EntityFrameworkCore;
using Ordering.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Data
{
	public interface IApplicationDBContext
	{
		public DbSet<Customer> Customers { get;  }
		public DbSet<Order> Orders { get;  }
		public DbSet<OrderItem> OrderItems { get; }
		public DbSet<Product> Products { get;  }

		public Task<int> SaveChangesAsync(CancellationToken cancellationToken);
	}
}
