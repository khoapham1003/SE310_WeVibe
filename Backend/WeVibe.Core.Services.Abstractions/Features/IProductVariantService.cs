using WeVibe.Core.Contracts.ProductVariant;

namespace WeVibe.Core.Services.Abstractions.Features
{
    public interface IProductVariantService
    {
        Task<ProductVariantDto> CreateAsync(CreateProductVariantDto createDto);
        Task<ProductVariantDto> GetProductVariantByIdAsync(int id);
        Task<IEnumerable<ProductVariantDto>> GetAllProductVariantsAsync();
        Task<ProductVariantDto> UpdateProductVariantAsync(int id, UpdateProductVariantDto updateDto);
        Task<bool> DeleteProductVariantByIdAsync(int id);
    }
}
