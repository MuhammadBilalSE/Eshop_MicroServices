


namespace Catalog.API.Products.GetById
{
	public record GetByIdRequest(Guid Id) : IRequest<GetProductByIdResponse>;

	public record GetProductByIdResponse(Product Product);

	public class GetByIDHadler (IDocumentSession session)
		: IRequestHandler<GetByIdRequest, GetProductByIdResponse>
	{
		public async Task<GetProductByIdResponse> Handle(GetByIdRequest request, CancellationToken cancellationToken)
		{
			var product = await session.LoadAsync<Product>(request);
			 if (product == null)
			{
				throw new ProductNotFoundException();
			}

			return new GetProductByIdResponse(product);

		}
	}
}
