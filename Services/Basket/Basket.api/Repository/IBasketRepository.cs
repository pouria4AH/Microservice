using System.Threading.Tasks;
using Basket.api.Entities;

namespace Basket.api.Repository
{
    public interface IBasketRepository
    {
        Task<ShoppingCart> GetUserBasket(string userName);
        Task<ShoppingCart> UpdateBasket(ShoppingCart basket);
        Task DeleteBasket(string userName);
    }
}
