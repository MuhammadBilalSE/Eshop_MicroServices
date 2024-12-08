using Basket.API.Models;
using BuildingBlocks.CQRS;
using MediatR;

namespace Basket.API.Basket.GetBasket
{
	public record GetBasketQuery (string userName) : IQuery<GetBasketResult>;
	public record GetBasketResult(ShoppingCart Cart);


	public class GetHandler () : IQueryHandler<GetBasketQuery, GetBasketResult>
	{
		
		public Task<GetBasketResult> Handle(GetBasketQuery request, CancellationToken cancellationToken)
		{
			throw new NotImplementedException(); 
		}
	}
}
