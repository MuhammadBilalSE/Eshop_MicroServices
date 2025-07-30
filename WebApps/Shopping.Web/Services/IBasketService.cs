using Refit;
using Shopping.Web.Models.Basket;

namespace Shopping.Web.Services
{
	public interface IBasketService
	{
		[Get("/basket-service/basket/{userName}")]
		Task<GetBasketRespone> GetBasket(string userName);

		[Post("/basket-service/basket")]
		Task<StoreBasketRespone> StoreBasket(StoreBasketRequest request);

		[Delete("/basket-service/basket/{userName}")]
		Task<GetBasketRespone> DeleteBasket(string userName);

		[Post("/basket-service/basket/checkout")]
		Task<CheckoutBasketResponse> CheckoutBasket(CheckoutBasketRequest userName);
	}
}
