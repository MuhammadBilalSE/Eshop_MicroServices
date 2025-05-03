using Basket.API.Data;
using Basket.API.Models;
using BuildingBlocks.CQRS;
using Discount.Grpc.Protos;
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

	public class StoreHandler(IBasketRepository basket, DiscountProtoService.DiscountProtoServiceClient discountproto) : MediatR.IRequestHandler<StoreBasketCommad, StoreBasketResult>
	{
		public async Task<StoreBasketResult> Handle(StoreBasketCommad request, CancellationToken cancellationToken)
		{
			foreach (var item in request.Cart.Items)
			{
				var coupon = await discountproto
				.GetDiscountAsync(new GetDiscountRequest { ProductName = item.ProductName }, cancellationToken: cancellationToken);
				item.Price -= coupon.Amount;
			}
			await basket.StoreBasket(request.Cart,cancellationToken);
			return new StoreBasketResult(request.Cart.UserName);
		}
	}
}
