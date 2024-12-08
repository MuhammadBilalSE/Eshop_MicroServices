using Basket.API.Basket.StoreBasket;
using Carter;
using Mapster;
using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace Basket.API.Basket.DeleteBasket
{
	//public record DeleteBasket(string UserName);
	public record DeleteBasketResponse(bool IsSuccess);
	public class DeleteBasketEndPoint : ICarterModule
	{
		public void AddRoutes(IEndpointRouteBuilder app)
		{
			app.MapDelete("/basket/{UserName}", async (string UserName, ISender sender) =>
			{
				var result = sender.Send(new DeleteBasketCommand(UserName));
				var response = result.Adapt<DeleteBasketResponse>();
				return Results.Ok(response);
			})
				.WithName("DeleteBasket")
				.Produces<BasketResponse>(StatusCodes.Status200OK)
				.ProducesProblem(StatusCodes.Status400BadRequest)
				.ProducesProblem(StatusCodes.Status404NotFound)
				.WithSummary("Delete Basket")
				.WithDescription("Delet Basket");
		}
	}
}
