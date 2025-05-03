using BuildingBlocks.Exceptions;
using FluentValidation;
using MediatR;
using Ordering.Application.Data;
using Ordering.Application.Dtos;
using Ordering.Domain.Models;
using Ordering.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Orders.Commands.UpdateOrder
{
	public record UpdateResult(bool Succeed);

	public class ValidateUpdate : AbstractValidator<UpdateOrderCommand>
	{
        public ValidateUpdate()
        {
            RuleFor(x => x.OrderDto.Id).NotEmpty().WithMessage("ID Should be Provided");
            RuleFor(x => x.OrderDto.OrderName).NotEmpty().WithMessage("OrderName Should be Provided");
            RuleFor(x => x.OrderDto.CustomerId).NotNull().WithMessage("CustomerID Should be Provided");
        }
    }
	public record UpdateOrderCommand(OrderDto OrderDto) : IRequest<UpdateResult>;



	//__________________________________________________________________

	public class UpdateOrderHandler(IApplicationDBContext context) : IRequestHandler<UpdateOrderCommand, UpdateResult>
	{
		public async Task<UpdateResult> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
		{
			var orderId = OrderId.Of(request.OrderDto.Id);
			var order = await context.Orders.FindAsync([orderId],cancellationToken:cancellationToken);

			if (order is  null) { 
				throw new NotFoundException($"Order Not Found {request.OrderDto.Id}");
			}
			UpdateOrderNow(order,request.OrderDto);
			context.Orders.Update(order);
			await context.SaveChangesAsync(cancellationToken);
			return new UpdateResult(true);
		}

		public void UpdateOrderNow(Order order, OrderDto orderDto)
		{
			var shippingAddress = Address.Of(orderDto.ShippingAddress.FirstName, orderDto.ShippingAddress.LastName, orderDto.ShippingAddress.EmailAddress,
				orderDto.ShippingAddress.AddressLine, orderDto.ShippingAddress.Country, orderDto.ShippingAddress.State, orderDto.ShippingAddress.ZipCode);
			var billingAddress = Address.Of(orderDto.BillingAddress.FirstName, orderDto.BillingAddress.LastName, orderDto.BillingAddress.EmailAddress,
				orderDto.BillingAddress.AddressLine, orderDto.BillingAddress.Country, orderDto.BillingAddress.State, orderDto.BillingAddress.ZipCode);
			var updatedPayment = Payment.Of(orderDto.Payment.CardName, orderDto.Payment.CardNumber, orderDto.Payment.Expiration, 
				orderDto.Payment.Cvv, orderDto.Payment.PaymentMethod);

			order.Update(
				orderName : OrderName.Of(orderDto.OrderName),
				shippingAddress : shippingAddress,
				billingAddress : billingAddress,
				payment : updatedPayment,
				status : orderDto.Status
				);


		}
	}

}
