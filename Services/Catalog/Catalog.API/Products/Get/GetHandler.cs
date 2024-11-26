
using Marten.Pagination;

namespace Catalog.API.Products.Get
{
	public record GetProductQuery(int? PageNumber = 1, int? PageSize = 10) : IRequest<GetProductResult>;

	public record GetProductResult(IEnumerable<Product> Products);

	public class GetHandler(IDocumentSession session, ILogger<GetHandler> logger) :
		IRequestHandler<GetProductQuery, GetProductResult>
	{
		public async Task<GetProductResult> Handle(GetProductQuery request, CancellationToken cancellationToken)
		{
			logger.LogInformation($"Get Product Handler.Handle with Query {request}");
			var products = await session.Query<Product>()
				.ToPagedListAsync(request.PageNumber??1,request.PageSize??10, cancellationToken); 
			return new GetProductResult(products);

		}
	}
}
