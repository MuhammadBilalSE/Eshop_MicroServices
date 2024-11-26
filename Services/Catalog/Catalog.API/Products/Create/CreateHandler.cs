using FluentValidation;

namespace Catalog.API.Products.CreateProduct;

public record CreateProductCommand(string Name, List<string> Category, string Description, string ImageFile, decimal Price)
	: IRequest<CreateProductResult>;
public record CreateProductResult(Guid Id);

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
	public CreateProductCommandValidator()
	{
		RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required");
		RuleFor(x => x.Category).NotEmpty().WithMessage("Category is required");
		RuleFor(x => x.ImageFile).NotEmpty().WithMessage("ImageFile is required");
		RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price must be greater than 0");
	}
}

public class CreateProductCommandHandler
	(IDocumentSession session)
	: IRequestHandler<CreateProductCommand, CreateProductResult>
{
	public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
	{
		//create Product entity from command object
		//save to database
		//return CreateProductResult result
		var product = new Product
		{
			Name = command.Name,
			Category = command.Category,
			Description = command.Description,
			ImageFile = command.ImageFile,
			Price = command.Price
		};

		//save to database
		session.Store(product);
		await session.SaveChangesAsync(cancellationToken);

		//return result
		return new CreateProductResult(product.Id);
		//"id": "0193136d-5ae0-41a9-a635-26cd79e12e54"
	}
}



//namespace Catalog.API.Products.CreateProduct;

//public record CreateProductRequest(string Name, List<string> Category, string Description, string ImageFile, decimal Price);

//public record CreateProductResponse(Guid Id);

//public class CreateProductEndpoint : ICarterModule
//{
//	public void AddRoutes(IEndpointRouteBuilder app)
//	{
//		app.MapPost("/products", 
//		async (CreateProductRequest request, ISender sender) =>
//		{
//			var command = request.Adapt<CreateProductCommand>();
//			var result = await sender.Send(command);
//			var response = result.Adapt<CreateProductResponse>();
//			return Results.Created($"/products/{response.Id}", response);
//		})
//		.WithName("CreateProduct")
//		.Produces<CreateProductResponse>(StatusCodes.Status201Created)
//		.ProducesProblem(StatusCodes.Status400BadRequest)
//		.WithSummary("Create Product")
//		.WithDescription("Creates a new product and returns its details.");
//	}
//}
