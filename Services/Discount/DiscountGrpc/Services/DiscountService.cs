using System;
using System.Threading.Tasks;
using AutoMapper;
using Discount.Grpc.Protos;
using Discount.Grpc.Repositories;
using Grpc.Core;
using Microsoft.Extensions.Logging;

namespace Discount.Grpc.Services
{
    public class DiscountService : DiscountProtoServices.DiscountProtoServicesBase
    {
        #region ctor

        private readonly IDiscountRepository _discountRepository;
        private readonly ILogger<DiscountService> _logger;
        public readonly IMapper _Mapper;
        public DiscountService(IDiscountRepository discountRepository, ILogger<DiscountService> logger, IMapper mapper)
        {
            _discountRepository = discountRepository;
            _logger = logger;
            _Mapper = mapper;
        }

        #endregion

        #region get discount

        public override async Task<CouponModel> GetDiscount(GetDiscountRequset request, ServerCallContext context)
        {
            var coupon = await _discountRepository.GetDiscount(request.ProductName);
            if (coupon == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound,
                    $"discount by name: {request.ProductName} Not found"));

            }
            _logger.LogInformation("discount is Retrived by product name");
            return _Mapper.Map<CouponModel>(coupon);

        }

        #endregion
    }
}
