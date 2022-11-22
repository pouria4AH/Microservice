using System.Threading.Tasks;
using Basket.api.Entities;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace Basket.api.Repository
{
    public class BasketRepository : IBasketRepository
    {
        #region ctor

        private readonly IDistributedCache _rediseCache;

        public BasketRepository(IDistributedCache rediseCache)
        {
            _rediseCache = rediseCache;
        }

        #endregion

        #region get user basket

        public async Task<ShoppingCart> GetUserBasket(string userName)
        {
            var basket = await _rediseCache.GetStringAsync(userName);

            if (string.IsNullOrEmpty(basket))
                return null;

            return JsonConvert.DeserializeObject<ShoppingCart>(basket);
        }
        #endregion

        #region update user basket

        public async Task<ShoppingCart> UpdateBasket(ShoppingCart basket)
        {
            await _rediseCache.SetStringAsync(basket.UserName, JsonConvert.SerializeObject(basket));

            return await GetUserBasket(basket.UserName);
        }
        #endregion

        #region delete user basket
        public async Task DeleteBasket(string userName)
        {
            await _rediseCache.RemoveAsync(userName);
        }
        #endregion

    }
}
