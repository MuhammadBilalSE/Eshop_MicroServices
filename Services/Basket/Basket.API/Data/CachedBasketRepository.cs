using Basket.API.Models;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Basket.API.Data
{
	public class CachedBasketRepository (IBasketRepository repository, IDistributedCache cache)
		: IBasketRepository
	{
		public  async Task<ShoppingCart> GetBasket(string UserName, CancellationToken cancellationToken)
		{
			var cachedbasket = await cache.GetStringAsync(UserName,cancellationToken);
			if (!string.IsNullOrEmpty(cachedbasket))
				return JsonSerializer.Deserialize<ShoppingCart>(cachedbasket)!;
			var basket = await repository.GetBasket(UserName,cancellationToken);
			await cache.SetStringAsync(UserName, JsonSerializer.Serialize(basket),cancellationToken);
			return basket;
		}

		public async Task<ShoppingCart> StoreBasket(ShoppingCart shoppingCart, CancellationToken cancellationToken)
		{
			await cache.SetStringAsync(shoppingCart.UserName,JsonSerializer.Serialize(shoppingCart), cancellationToken);
			await repository.StoreBasket(shoppingCart,cancellationToken);
			return shoppingCart;
		}
		public async Task<bool> DeleteBasket(string UserName, CancellationToken cancellationtoken)
		{
			await repository.DeleteBasket(UserName, cancellationtoken);
			await cache.RemoveAsync(UserName, cancellationtoken);
			return true;
		}
	}
}
