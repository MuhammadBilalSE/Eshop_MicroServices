
using Catalog.API.Products.GetById;

namespace Catalog.API.Products.Delete
{
	//public record DeleteProduct(Guid Id);
	public record DeleteResponse(bool Success);

	public class DeleteEndPoint : ICarterModule
	{
		public void AddRoutes(IEndpointRouteBuilder app)
		{
			app.MapDelete("/products/{id}",
				async (Guid id, ISender sender) =>
				{
					var result = await sender.Send(new DeleteProductCommand(id));
					var response = result.Adapt<DeleteResponse>();
					return Results.Ok(response);
				})
				.WithName("DeleteProduct")
				.Produces<DeleteResponse>(StatusCodes.Status201Created)
				.ProducesProblem(StatusCodes.Status400BadRequest)
				.WithSummary("DeleteProduct")
				.WithDescription("DeleteProduct"); ;
		}
	}
}
