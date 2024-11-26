namespace Catalog.API.Products.GetById
{
	public record GetByCategoryRequest(string Category) : IRequest<GetProductByCategoryResponse>;

	public record GetProductByCategoryResponse(IEnumerable<Product> Products);

	public class GetByCategoryHadler(IDocumentSession session, ILogger<GetByCategoryRequest> logger)
		: IRequestHandler<GetByCategoryRequest, GetProductByCategoryResponse>
	{
		public async Task<GetProductByCategoryResponse> Handle(GetByCategoryRequest request, CancellationToken cancellationToken)
		{
			logger.LogInformation($"Handling Get Product By Category {request.Category}");

			var products = await session.Query<Product>()
				.Where(x =>x.Category.Contains(request.Category))
				.ToListAsync(cancellationToken);
			return new GetProductByCategoryResponse(products);

		}
	}
}
