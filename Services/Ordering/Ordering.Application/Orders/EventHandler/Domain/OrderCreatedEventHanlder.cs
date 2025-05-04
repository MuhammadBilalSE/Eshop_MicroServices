using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.FeatureManagement;
using Ordering.Application.Extensions;
using Ordering.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Orders.EventHandler.Domain
{
	public class OrderCreatedEventHanlder(IPublishEndpoint publish,IFeatureManager featureManager ,ILogger<OrderUpdatedEvent> logger) 
			: INotificationHandler<OrderCreatedEvent>
	{
		public async Task Handle(OrderCreatedEvent domainEvent, CancellationToken cancellationToken)
		{
			logger.LogInformation($"Domain Event Handled : {domainEvent.GetType().Name}");

			if (await featureManager.IsEnabledAsync("OrderFulfilement"))
			{
				var orderCreatedInegrationEvent = domainEvent.Order.ToOrderDto();
				await publish.Publish(orderCreatedInegrationEvent, cancellationToken);
			}
			
		}
	}
}
