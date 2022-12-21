using System.Threading.Tasks;
using Discount.api.Entities;

namespace Discount.api.Repositories
{
    public interface IDiscountRepository
    {
        Task<Coupon> GetDiscount(string productName);
        Task<bool> CrateDiscount(Coupon coupon);
        Task<bool> UpdateDiscount(Coupon coupon);
        Task<bool> DeleteDiscount(string productName);
    }
}
