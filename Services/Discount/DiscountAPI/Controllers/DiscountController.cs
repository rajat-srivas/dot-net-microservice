using DiscountAPI.Entities;
using DiscountAPI.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;

namespace DiscountAPI.Controllers
{
	[Route("api/v1/[controller]")]
	[ApiController]
	public class DiscountController : ControllerBase
	{
		private readonly IDiscountRepository _discount;
		public DiscountController(IDiscountRepository discount)
		{
			_discount = discount;	
		}

		[HttpGet("{code}")]
		[ProducesResponseType(typeof(ActionResult<Coupon>),(int)HttpStatusCode.OK)]

		public async Task<ActionResult<Coupon>> VerifyCoupon(string code)
		{
			var coupons = await _discount.VerifyCoupon(code);
			return Ok(coupons);
		}

		[HttpPost]
		[ProducesResponseType(typeof(ActionResult<bool>), (int)HttpStatusCode.OK)]
		public async Task<ActionResult<bool>> CreateDiscountCoupon(Coupon coupon)
		{
			var response = await _discount.CreateDiscountCoupon(coupon);
			return Ok(response);
		}

		[HttpPut]
		[ProducesResponseType(typeof(ActionResult<bool>), (int)HttpStatusCode.OK)]
		public async Task<ActionResult<bool>> UpdateDiscountCoupon(Coupon coupon)
		{
			var response = await _discount.UpdateDiscountCoupon(coupon);
			return Ok(response);
		}

		[HttpDelete("{code}")]
		[ProducesResponseType(typeof(ActionResult<bool>), (int)HttpStatusCode.OK)]
		public async Task<ActionResult<bool>> DeleteDiscountCoupon(string code)
		{
			var response = await _discount.DeleteDiscountCopon(code);
			return Ok(response);
		}
	}
}
