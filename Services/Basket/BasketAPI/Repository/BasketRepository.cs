using BasketAPI.Entities;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BasketAPI.Repository
{
	public class BasketRepository : IBasketRepository
	{
		private readonly IDistributedCache _redisCache;

		public BasketRepository(IDistributedCache redisCache)
		{
			_redisCache = redisCache;
		}

		public async Task DeleteBasket(string userName)
		{
			await _redisCache.RemoveAsync(userName);
		}

		public async Task<ShoppingCart> GetBasket(string userName)
		{
			string basketJson = await  _redisCache.GetStringAsync(userName);
			if (string.IsNullOrEmpty(basketJson)) return null;

			var basket = JsonConvert.DeserializeObject<ShoppingCart>(basketJson);
			return basket;
		}

		public async Task<ShoppingCart> UpdateBasket(ShoppingCart basket)
		{
			string basketJson = JsonConvert.SerializeObject(basket);
			await _redisCache.SetStringAsync(basket.UserName, basketJson);
			return await GetBasket(basket.UserName);
		}
	}
}
