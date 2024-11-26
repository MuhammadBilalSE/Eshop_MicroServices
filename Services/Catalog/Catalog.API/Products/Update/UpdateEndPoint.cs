
using Catalog.API.Products.GetById;

namespace Catalog.API.Products.Update
{
	public record UpdateProdcutRequest (Guid Id, string Name, List<string> Category, string Description, string ImageFile, decimal Price);
	public record UpdateProductResponse (bool success);

	public class UpdateEndPoint : ICarterModule
	{
		public void AddRoutes(IEndpointRouteBuilder app)
		{
			app.MapPut("/products",
				async (UpdateProdcutRequest request , ISender sender) =>
				{
					var command = request.Adapt<UpdateProductCommand>();
					var result = await sender.Send(command);
					var response = result.Adapt<UpdateProductResponse>();
					return Results.Ok(response);
				})
				.WithName("UpdateProduct")
				.Produces<GetProductResponse>(StatusCodes.Status201Created)
				.ProducesProblem(StatusCodes.Status400BadRequest)
				.WithSummary("Update Product")
				.WithDescription("Update Product");
		}
	}
}
