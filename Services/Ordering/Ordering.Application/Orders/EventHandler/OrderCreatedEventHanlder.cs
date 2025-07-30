using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Orders.EventHandler
{
	public class OrderCreatedEventHanlder(ILogger<OrderUpdatedEvent> logger) : INotificationHandler<OrderCreatedEvent>
	{
		public Task Handle(OrderCreatedEvent notification, CancellationToken cancellationToken)
		{
			logger.LogInformation($"Domain Event Handled : {notification.GetType().Name}");
			return Task.CompletedTask;	
		}
	}
}
