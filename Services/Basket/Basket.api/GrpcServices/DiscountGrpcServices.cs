using System.Threading.Tasks;
using Discount.Grpc.Protos;

namespace Basket.api.GrpcServices
{
    public class DiscountGrpcServices
    {
        #region ctor
        private readonly DiscountProtoServices.DiscountProtoServicesClient _discountClient;

        public DiscountGrpcServices(DiscountProtoServices.DiscountProtoServicesClient discountClient)
        {
            _discountClient = discountClient;
        }
        #endregion

        #region get discount

        public async Task<CouponModel> GetDiscount(string productName)
        {
            var discountRequest = new GetDiscountRequset { ProductName = productName };
            return await _discountClient.GetDiscountAsync(discountRequest);
        }
        #endregion
    }
}
