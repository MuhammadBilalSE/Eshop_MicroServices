using Basket.API.Basket.GetBasket;
using Basket.API.Models;
using Carter;
using Mapster;
using MediatR;

namespace Basket.API.Basket.StoreBasket
{
	public record BasketRequest(ShoppingCart ShoppingCart);
	public record BasketResponse(string UserName);
	public class StoreEndPoint : ICarterModule
	{
		public void AddRoutes(IEndpointRouteBuilder app)
		{
			app.MapPost("/basket", async(BasketRequest request, ISender sender) => {
				var command = request.Adapt<StoreBasketCommad>();
				var result = await sender.Send(command);
				var response = request.Adapt<BasketResponse>();
				return Results.Created($"/basket/{response.UserName}", response);
			})
				.WithName("StoreBasket")
				.Produces<BasketResponse>(StatusCodes.Status200OK)
				.ProducesProblem(StatusCodes.Status400BadRequest)
				.WithSummary("Store Basket")
				.WithDescription("Store Basket");
		}
	}
}
