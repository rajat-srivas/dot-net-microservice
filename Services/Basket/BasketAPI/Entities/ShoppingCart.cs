using System.Collections.Generic;

namespace BasketAPI.Entities
{
	public class ShoppingCart
	{
		public ShoppingCart()
		{

		}
		public ShoppingCart(string _userName)
		{
			UserName = _userName;
		}
		public string UserName { get; set; }

		public List<ShoppingCartItems> Items { get; set; } = new List<ShoppingCartItems>();

		public decimal TotalPrice
		{
			get
			{
				decimal totalPrice = 0;
				Items.ForEach(x =>
				{
					totalPrice += x.Price * x.Quantity;
				});

				return totalPrice;
			}
		}
	}

	public class ShoppingCartItems
	{
		public int Quantity { get; set; }

		public string Color { get; set; }

		public decimal Price { get; set; }

		public string ProductId { get; set; }

		public string ProductName { get; set; }
	}
}
