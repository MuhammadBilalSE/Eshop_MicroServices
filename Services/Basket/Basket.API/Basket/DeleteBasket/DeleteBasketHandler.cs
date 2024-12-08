using MediatR;
using System.Diagnostics.CodeAnalysis;
using System.Windows.Input;

namespace Basket.API.Basket.DeleteBasket
{
	public record DeleteBasketCommand(string UserName ) : IRequest<DeleteResponse>;
	public record DeleteResponse (bool IsSuccess);
	public class DeleteBasketHandler : IRequestHandler<DeleteBasketCommand, DeleteResponse>
	{
		public async Task<DeleteResponse> Handle(DeleteBasketCommand request, CancellationToken cancellationToken)
		{
			return new DeleteResponse(true);
		}
	}
}
