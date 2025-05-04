using Basket.API.Data;
using Basket.API.Dts;
using BuildingBlock.Messaging.Events;
using FluentValidation;
using Mapster;
using MassTransit;
using MediatR;
using NetTopologySuite.Triangulate.QuadEdge;

namespace Basket.API.Basket.CheckoutBasket
{
	public record CheckoutBasketCommand(BasketCheckoutDtso BasketCheckoutDtso) : IRequest<CheckoutBasketResult>;
	
	public record CheckoutBasketResult(bool Succeed);

	public class ValidateCheckoutBasket : AbstractValidator<CheckoutBasketCommand>
	{
        public ValidateCheckoutBasket()
        {
			RuleFor(b => b.BasketCheckoutDtso).NotNull().WithMessage("Basket Should Be Filled");
			RuleFor(b => b.BasketCheckoutDtso.UserName).NotNull().WithMessage("Username Should Be Filled");
        }
    }

	public class CheckoutBasketHandler (IBasketRepository repository, IPublishEndpoint publishEndpoint) : IRequestHandler<CheckoutBasketCommand, CheckoutBasketResult>
	{
		public async Task<CheckoutBasketResult> Handle(CheckoutBasketCommand command, CancellationToken cancellationToken)
		{
			var basket = await  repository.GetBasket(command.BasketCheckoutDtso.UserName, cancellationToken: cancellationToken);
			if (basket == null) {
				return new CheckoutBasketResult(false);
			}
			var eventmessage = command.BasketCheckoutDtso.Adapt<BasketCheckoutEvent>();
			eventmessage.TotalPrice = basket.TotalPrice;

			await publishEndpoint.Publish(eventmessage, cancellationToken : cancellationToken);
			await repository.DeleteBasket(command.BasketCheckoutDtso.UserName, cancellationToken);

			return new CheckoutBasketResult(true);
		}
	}
}
