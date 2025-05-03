using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ordering.Application.Data;
using Ordering.Infrastructure.Data;
using Ordering.Infrastructure.Data.Intercepters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Infrastructure
{
	public static class DependencyInjection
	{
		public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
		{
			var connectionstring = configuration.GetConnectionString("Database");

			services.AddScoped<ISaveChangesInterceptor, AuditableIntercepter>();
			services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();
			services.AddDbContext<ApplicationDBContext>((sp,options)=>
			{
				options.AddInterceptors(sp.GetService<ISaveChangesInterceptor>());
				options.UseSqlServer(connectionstring);
			}) ;
			services.AddScoped<IApplicationDBContext, ApplicationDBContext>();
			return services;
		} 
	}
}
