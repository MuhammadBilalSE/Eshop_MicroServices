
using FluentValidation;
using Marten.Util;

namespace Catalog.API.Products.Update
{
	public record UpdateProductCommand(Guid Id, string Name, List<string> Category, string Description, string ImageFile, decimal Price)
	: IRequest<UpdateProductResult>;
	public record UpdateProductResult(bool success);
	public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
	{
		public UpdateProductCommandValidator()
		{
			RuleFor(command => command.Id).NotEmpty().WithMessage("Product ID is required");

			RuleFor(command => command.Name)
				.NotEmpty().WithMessage("Name is required")
				.Length(2, 150).WithMessage("Name must be between 2 and 150 characters");

			RuleFor(command => command.Price)
				.GreaterThan(0).WithMessage("Price must be greater than 0");
		}
	}
	public class UpdateHandler(IDocumentSession session)
		: IRequestHandler<UpdateProductCommand, UpdateProductResult>
	{
		public async Task<UpdateProductResult> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
		{
			var product =await session.LoadAsync<Product>(request.Id, cancellationToken);
			if (product is null)
			{
				throw new ProductNotFoundException();
			}
			product.Name = request.Name;
			product.Description = request.Description;
			product.Category = request.Category;
			product.Price = request.Price;
			product.ImageFile = request.ImageFile;
			session.Update(product);
			await session.SaveChangesAsync(cancellationToken);

			return new UpdateProductResult(true);


		}
	}
}
