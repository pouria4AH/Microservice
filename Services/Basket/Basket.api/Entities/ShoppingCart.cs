using System.Collections.Generic;
using System.Linq;

namespace Basket.api.Entities
{
    public class ShoppingCart
    {
        public ShoppingCart()
        {

        }

        public ShoppingCart(string userName)
        {
            userName = UserName;
        }
        public string UserName { get; set; }
        public List<ShoppingCartItem> ShoppingCartItems { get; set; }
        public decimal TotalPrice
        {
            get
            {
                decimal totalPrice = 0;
                if (ShoppingCartItems != null && ShoppingCartItems.Any())
                {
                    foreach (ShoppingCartItem item in ShoppingCartItems)
                    {
                        totalPrice += item.Price * item.Quantity;
                    }
                }
                return totalPrice;
            }
        }
    }
}
