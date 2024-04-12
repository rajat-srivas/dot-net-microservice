using BasketAPI.Entities;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BasketAPI.Repository
{
	public class BasketRepository : IBasketRepository
	{
		private readonly IDistributedCache _redisCache;
		//private readonly IConnectionMultiplexer _redisCache;
		IDatabase _redisDb = null;
		IConfiguration _config;

		public BasketRepository(IDistributedCache redisCache)
		{
			_redisCache = redisCache;
			//_redisDb = _redisCache.GetDatabase();
		}
		

		public Task DeleteBasket(string userName)
		{
			throw new NotImplementedException();
		}

		public async Task<ShoppingCart> GetBasket(string userName)
		{
			string basketJson = await _redisCache.GetStringAsync(userName);
			if (string.IsNullOrEmpty(basketJson)) return null;
			var basket = JsonConvert.DeserializeObject<ShoppingCart>(basketJson);
			return basket;
		}

		public async Task<ShoppingCart> UpdateBasket(ShoppingCart basket)
		{
			string basketJson = JsonConvert.SerializeObject(basket);
			await _redisCache.SetStringAsync(basket.UserName, basketJson, new DistributedCacheEntryOptions { AbsoluteExpiration = DateTime.Now.AddMinutes(30) });
			return await GetBasket(basket.UserName);
		}
	}
}
