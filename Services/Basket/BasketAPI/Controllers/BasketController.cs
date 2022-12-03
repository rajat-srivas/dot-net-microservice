using BasketAPI.Entities;
using BasketAPI.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;

namespace BasketAPI.Controllers
{
	[Route("api/v1/[controller]")]
	[ApiController]
	public class BasketController : ControllerBase
	{
		private readonly IBasketRepository _basketRepository;
		public BasketController(IBasketRepository basketRepository)
		{
			_basketRepository = basketRepository;
		}

		[HttpGet("{userName}", Name ="GetBasket")]
		[ProducesResponseType(typeof(ShoppingCart),(int)HttpStatusCode.OK)]
		public async Task<ActionResult<ShoppingCart>> GetBasket(string userName)
		{
			var basket = await _basketRepository.GetBasket(userName);
			return Ok(basket ?? new ShoppingCart(userName));
		}

		[HttpPost]
		[ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
		public async Task<ActionResult<ShoppingCart>> UpdateBasket([FromBody]ShoppingCart basket)
		{
			var updatedBasket = await _basketRepository.UpdateBasket(basket);
			return Ok(updatedBasket);
		}

		[HttpDelete("{userName}")]
		public async Task DeleteBasket(string userName)
		{
			await _basketRepository.DeleteBasket(userName);
		}
	}
}
