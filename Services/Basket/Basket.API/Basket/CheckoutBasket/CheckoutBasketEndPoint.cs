using Basket.API.Dts;
using Carter;
using Mapster;
using MediatR;

namespace Basket.API.Basket.CheckoutBasket
{

	public record CheckoutBasketRequest(BasketCheckoutDtso BasketCheckoutDtob);

	public record CheckoutBasketResposne(bool Succeed);
	public class CheckoutBasketEndPoint() : ICarterModule
	{
		public void AddRoutes(IEndpointRouteBuilder app)
		{
			app.MapPost("/basket/checkout", async (CheckoutBasketRequest request, ISender sender) =>
			{
				var commad = request.Adapt<CheckoutBasketRequest>();
				var result = sender.Send(commad);
				var res = result.Adapt<CheckoutBasketResposne>();
				return Results.Ok(res);
			})
		.WithName("CheckoutBasket")
		.Produces<CheckoutBasketResposne>(StatusCodes.Status201Created)
		.ProducesProblem(StatusCodes.Status400BadRequest)
		.WithSummary("Checkout Basket")
		.WithDescription("Checkout Basket");
		}
	}
}
