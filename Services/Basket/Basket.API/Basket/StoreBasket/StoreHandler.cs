using Basket.API.Models;
using BuildingBlocks.CQRS;
using FluentValidation;
using MediatR;
using System.Windows.Input;

namespace Basket.API.Basket.StoreBasket
{
	public record StoreBasketCommad(ShoppingCart Cart) : IRequest<StoreBasketResult> ;
	public record StoreBasketResult(string UserName);
	
	public class StoreBasketValidator : AbstractValidator<StoreBasketCommad>
	{
        public StoreBasketValidator()
        {
			RuleFor(x =>x.Cart).NotNull().WithMessage("Cart Should be Filled");
			RuleFor(x =>x.Cart.UserName).NotEmpty().WithMessage("Name Should be Filled");
        }
    }

	public class StoreHandler : IRequestHandler<StoreBasketCommad, StoreBasketResult>
	{
		public async Task<StoreBasketResult> Handle(StoreBasketCommad request, CancellationToken cancellationToken)
		{
			ShoppingCart cart = request.Cart;
			return new StoreBasketResult("Billyy");
			//throw new NotImplementedException();
		}
	}
}
