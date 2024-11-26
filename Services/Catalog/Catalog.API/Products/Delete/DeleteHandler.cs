
using FluentValidation;

namespace Catalog.API.Products.Delete
{
	public record DeleteProductCommand(Guid Id) : IRequest<DeleteProductResponse>;
	public record DeleteProductResponse(bool Success);

	public class DeleteProductCommandValidator : AbstractValidator<DeleteProductCommand>
	{
		public DeleteProductCommandValidator()
		{
			RuleFor(x => x.Id).NotEmpty().WithMessage("Product ID is required");
		}
	}
	public class DeleteHandler(IDocumentSession session)
		: IRequestHandler<DeleteProductCommand, DeleteProductResponse>
	{
		public async Task<DeleteProductResponse> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
		{
			session.Delete<Product>(request.Id);
			await session.SaveChangesAsync();
			return new DeleteProductResponse(true);
		}
	}
}
