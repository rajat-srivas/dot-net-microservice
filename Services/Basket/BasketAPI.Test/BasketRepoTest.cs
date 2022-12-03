using BasketAPI.Entities;
using BasketAPI.Repository;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using System.Threading.Tasks;

namespace BasketAPI.Test
{
	public class BasketRepoTest
	{
		Mock<BasketRepository> _basketRepoMock;
		IDistributedCache _inMeoryCache;

		[SetUp]
		public void Setup()
		{
			var opts = Options.Create<MemoryDistributedCacheOptions>(new MemoryDistributedCacheOptions());
			_inMeoryCache = new MemoryDistributedCache(opts);

		}

		[Test]
		[TestCase("CorrectKey", true)]
		[TestCase("InCorrectKey", false)]
		public async Task WhenKey_Returns_Values(string key, bool expectedResult)
		{
			var corectKeyCart = MockShoppingCart(key);

			_inMeoryCache.SetString("CorrectKey", JsonConvert.SerializeObject(corectKeyCart));
			_inMeoryCache.SetString("InCorrectKey", JsonConvert.SerializeObject(new ShoppingCart("InCorrectKey")));

			_basketRepoMock = new Mock<BasketRepository>(_inMeoryCache);
			_basketRepoMock.CallBase = true;
			var result = await _basketRepoMock.Object.GetBasket(key);

			_basketRepoMock.Verify();

			Assert.AreEqual(true, result is ShoppingCart);
			Assert.AreEqual(true, result != null);

			Assert.Pass();

		}

		[Test]
		public async Task WhenNewBasket_Returns_KeyValue()
		{

			var cartToAdd = MockShoppingCart("NewKey");
			
			_basketRepoMock = new Mock<BasketRepository>(_inMeoryCache);
			_basketRepoMock.CallBase = true;

			var result = await _basketRepoMock.Object.UpdateBasket(cartToAdd);

			_basketRepoMock.Verify();

			Assert.AreEqual(true, result is ShoppingCart);
			Assert.AreEqual(true, result.UserName == "NewKey");
			Assert.AreEqual(true, result.Items.Count == 1);

			Assert.Pass();
		}

		[Test]
		public async Task WhenExistingBasket_Returns_UpdatedKeyValue()
		{
			var existingKeyValue = MockShoppingCart("ExistingKey");
			_inMeoryCache.SetString("ExistingKey", JsonConvert.SerializeObject(existingKeyValue));
			var cartToUpdate = MockShoppingCart("ExistingKey");
			cartToUpdate.Items[0].Quantity = 2;
			cartToUpdate.Items[0].Color = "yellow";

			_basketRepoMock = new Mock<BasketRepository>(_inMeoryCache);
			_basketRepoMock.CallBase = true;

			var result = await _basketRepoMock.Object.UpdateBasket(cartToUpdate);
			_basketRepoMock.Verify();

			Assert.AreEqual(true, result is ShoppingCart);
			Assert.AreEqual(false, existingKeyValue.Items[0].Quantity == result.Items[0].Quantity);

			_basketRepoMock.Verify();

		}

		[Test]
		[TestCase("ExistingKey", true)]
		public async Task WhenExistingKey_DeletesTheValue(string key, bool expectedResult)
		{
			var existingKeyValue = MockShoppingCart("ExistingKey");
			_inMeoryCache.SetString("ExistingKey", JsonConvert.SerializeObject(existingKeyValue));
			_basketRepoMock = new Mock<BasketRepository>(_inMeoryCache);
			_basketRepoMock.CallBase = true;

			 await _basketRepoMock.Object.DeleteBasket(key);
			var result = await _basketRepoMock.Object.GetBasket(key);
			_basketRepoMock.Verify();

			Assert.AreEqual(expectedResult, result == null);
		}



		private ShoppingCart MockShoppingCart(string key)
		{
			return new  ShoppingCart()
			{
				UserName = key,
				Items = new System.Collections.Generic.List<ShoppingCartItems>()
				{
					new ShoppingCartItems()
					{
						Quantity = 1,
						Color = "blue",
						Price = 9,
						ProductId = "1020301",
						ProductName = $"{key} Product Name"

					}
				}
			};
		}
	}
}