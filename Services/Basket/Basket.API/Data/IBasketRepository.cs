using Basket.API.Models;

namespace Basket.API.Data
{
	public interface IBasketRepository
	{
		Task<ShoppingCart> GetBasket(string UserName, CancellationToken cancellationToken);
		Task<ShoppingCart> StoreBasket(ShoppingCart shoppingCart, CancellationToken cancellationToken);
		Task<bool> DeleteBasket(string UserName, CancellationToken cancellationtoken);
	}
}
