
using Catalog.API.Products.CreateProduct;

namespace Catalog.API.Products.GetById
{
	//public record GetProductRequest(Guid Id);
	public record GetProductCategoryResponse(IEnumerable<Product> Products);

	public class GetByCategoryEndPoint : ICarterModule
	{
		public void AddRoutes(IEndpointRouteBuilder app)
		{
			app.MapGet("/prodcuts/category/{category}",
				async (string category, ISender sender) =>
				{
					var products = await sender.Send(new GetByCategoryRequest(category));
					var result = products.Adapt<GetProductCategoryResponse>();
					return Results.Ok(result);
				})
				.WithName("GetProductByCategory")
				.Produces<GetProductResponse>(StatusCodes.Status201Created)
				.ProducesProblem(StatusCodes.Status400BadRequest)
				.WithSummary("GetProductById")
				.WithDescription("GetProductById");
		}
	}
}
