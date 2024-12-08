using Basket.API.Exception;
using Basket.API.Models;
using Marten;
using System.Threading;

namespace Basket.API.Data
{
	public class BasketRepository(IDocumentSession session) : IBasketRepository
	{
		public async Task<bool> DeleteBasket(string UserName, CancellationToken cancellationtoken)
		{
			session.Delete<ShoppingCart>(UserName);
			session.SaveChangesAsync();
			return true;
		}

		public async Task<ShoppingCart> GetBasket(string UserName, CancellationToken cancellationToken)
		{
			var Basket =await session.LoadAsync<ShoppingCart>(UserName,cancellationToken);
			return
				Basket is null ?
				throw new BasketNotFoundException(UserName)
				: Basket;
		}

		public async Task<ShoppingCart> StoreBasket(ShoppingCart shoppingCart, CancellationToken cancellationToken)
		{
			session.Store(shoppingCart);
			session.SaveChangesAsync();
			return shoppingCart;
		}
	}
}
