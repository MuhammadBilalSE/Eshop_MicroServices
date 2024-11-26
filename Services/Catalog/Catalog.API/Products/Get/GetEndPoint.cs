
using Catalog.API.Products.CreateProduct;

namespace Catalog.API.Products.Get
{
	public record GetRequest(int? PageNumber=1, int? PageSize=10);
	public record GetResponse(IEnumerable<Product> Products);
	public class GetEndPoint : ICarterModule
	{
		public void AddRoutes(IEndpointRouteBuilder app)
		{
			app.MapGet("/products",
				async([AsParameters] GetRequest request, ISender sender) => {
					var query = request.Adapt<GetProductQuery>();
					var products = await sender.Send(query);
					var response = products.Adapt<GetResponse>();
					return Results.Ok(response);

			})
		.WithName("GetProduct")
		.Produces<GetResponse>(StatusCodes.Status201Created)
		.ProducesProblem(StatusCodes.Status400BadRequest)
		.WithSummary("Get Product")
		.WithDescription("Get Product"); ;
		}
	}
}
