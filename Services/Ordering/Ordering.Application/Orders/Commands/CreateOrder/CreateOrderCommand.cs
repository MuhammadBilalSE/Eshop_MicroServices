using FluentValidation;
using MediatR;
using Ordering.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Orders.Commands.CreateOrder
{
    public record CreateOrderResult(Guid Id);

    public class ValidateOrderCommand : AbstractValidator<CreateOrderCommand>
    {
        public ValidateOrderCommand()
        {
            RuleFor(x => x.order.OrderName).NotEmpty().WithMessage("Name Is Required");
            RuleFor(x => x.order.CustomerId).NotNull().WithMessage("CustomerID Is Required");
            RuleFor(x => x.order.OrderItems).NotEmpty().WithMessage("Add some Items in Order");
        }
    }

    public record CreateOrderCommand(OrderDto order) : IRequest<CreateOrderResult>;
}
