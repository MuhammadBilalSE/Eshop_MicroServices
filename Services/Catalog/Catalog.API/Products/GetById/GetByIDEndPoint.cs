
using Catalog.API.Products.CreateProduct;

namespace Catalog.API.Products.GetById
{
	//public record GetProductRequest(Guid Id);
	public record GetProductResponse(Product Product);

	public class GetByIDEndPoint : ICarterModule
	{
		public void AddRoutes(IEndpointRouteBuilder app)
		{
			app.MapGet("/prodcuts/{id}",
				async (Guid id, ISender sender) =>
				{
					var product = await sender.Send(new GetByIdRequest(id));
					var result = product.Adapt<GetProductResponse>();
					return Results.Ok(result);
				})
				.WithName("GetProductById")
				.Produces<GetProductResponse>(StatusCodes.Status201Created)
				.ProducesProblem(StatusCodes.Status400BadRequest)
				.WithSummary("GetProductById")
				.WithDescription("GetProductById");
		}
	}
}
