using System.Net;
using System.Threading.Tasks;
using Basket.api.Entities;
using Basket.api.GrpcServices;
using Basket.api.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Basket.api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        #region ctor
        private readonly IBasketRepository _basketRepository;
        private readonly DiscountGrpcServices _discountGrpcServices;
        public BasketController(IBasketRepository basketRepository, DiscountGrpcServices discountGrpcServices)
        {
            _basketRepository = basketRepository;
            _discountGrpcServices = discountGrpcServices;
        }

        #endregion

        #region get basket
        [HttpGet("{userName}" , Name = "GetBasket")]
        [ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK )]
        public async Task<ActionResult<ShoppingCart>> GetBasket(string userName)
        {
            var basket = await _basketRepository.GetUserBasket(userName);
            return Ok(basket ?? new ShoppingCart(userName));
        }
        #endregion

        #region update basket
        [HttpPost]
        [ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShoppingCart>> UpdateBasket([FromBody]ShoppingCart basket)
        {
            foreach (var item in basket.ShoppingCartItems)
            {
                var coupon = await _discountGrpcServices.GetDiscount(item.ProductName);
                item.Price -= coupon.Amount;
            }
            return Ok(await _basketRepository.UpdateBasket(basket));
        }
        #endregion

        #region delete basket
        [HttpDelete("{userName}" , Name = "DeleteBasket")]
        public async Task<IActionResult> DeleteBasket(string userName)
        {
            await _basketRepository.DeleteBasket(userName);
            return Ok();
        }
        #endregion
    }
}
