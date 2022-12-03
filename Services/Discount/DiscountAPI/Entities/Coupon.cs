using System.ComponentModel.DataAnnotations;

namespace DiscountAPI.Entities
{
	public class Coupon
	{
		[Key]
		public int Id { get; set; }
		public string CouponCode { get; set; }

		public string Description { get; set; }

		public int Amount { get; set; }

		public string Status { get; set; }
	}
}
