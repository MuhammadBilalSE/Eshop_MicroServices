using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Ordering.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Infrastructure.Data.Intercepters
{
	public class DispatchDomainEventsInterceptor(IMediator mediator)
		: SaveChangesInterceptor
	{
		public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
		{
			DispatchDomainEvents(eventData.Context);
			return base.SavingChanges(eventData, result);
		}

		public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
		{
			await DispatchDomainEvents(eventData.Context);
			return await base.SavingChangesAsync(eventData, result, cancellationToken);
		}
		
		public async Task DispatchDomainEvents(DbContext context)
		{
			if (context is null) return;
			var aggregate = context.ChangeTracker
				.Entries<IAggregate>()
				.Where(x => x.Entity.DomainEvents.Any())
				.Select(x => x.Entity);
			var domainEvents = aggregate
				.SelectMany(x =>x.DomainEvents)
				.ToList();

			aggregate.ToList().ForEach(x => x.ClearDomainEvents()) ;

			foreach (var item in domainEvents)
			{
				await mediator.Publish(item);
			}



		}
	}
}
