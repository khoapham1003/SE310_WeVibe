using WeVibe.Core.Contracts.Discount;

namespace WeVibe.Core.Services.Abstractions.Features
{
    public interface IDiscountService
    {
        Task<DiscountDto> CreateDiscountAsync(CreateDiscountDto discountDto);
        Task<DiscountDto> GetDiscountByIdAsync(int discountId);
        Task<IEnumerable<DiscountDto>> GetAllDiscountsAsync();
        Task<DiscountDto> UpdateDiscountAsync(int discountId, DiscountDto discountDto);
        Task<bool> DeleteAsync(int discountId);
    }
}
