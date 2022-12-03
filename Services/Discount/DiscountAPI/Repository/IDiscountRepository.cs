using DiscountAPI.Entities;
using System.Threading.Tasks;

namespace DiscountAPI.Repository
{
	public interface IDiscountRepository
	{
		Task<Coupon> VerifyCoupon(string couponCode);

		Task<bool> CreateDiscountCoupon(Coupon coupon);

		Task<bool> UpdateDiscountCoupon(Coupon coupon);

		Task<bool> DeleteDiscountCopon(string couponCode);



	}
}
