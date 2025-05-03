using BuildingBlocks.Exceptions;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Ordering.Application.Data;
using Ordering.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Orders.Commands.DeleteOrder
{
	public record DeleteOrderCommand(Guid OrderId) : IRequest<DeleteResult>;
	
	public record DeleteResult (bool Succeed);

	public class validateDeleteCommand : AbstractValidator<DeleteOrderCommand>
	{
        public validateDeleteCommand()
        {
			RuleFor(x => x.OrderId).NotNull().WithMessage("IS is Null");
        }
    }

	public class DeleteHandler (IApplicationDBContext dBContext): IRequestHandler<DeleteOrderCommand, DeleteResult>
	{
		public async Task<DeleteResult> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
		{
			var orderId = OrderId.Of(request.OrderId);
			var order = await dBContext.Orders.FindAsync([orderId], cancellationToken: cancellationToken);
			if (order is null)
			{
				throw new NotFoundException($"Order with ID {request.OrderId} not found");
			}
			dBContext.Orders.Remove(order);
			await dBContext.SaveChangesAsync(cancellationToken);
			return new DeleteResult(true);
		}
	}
}
