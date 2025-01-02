using WeVibe.Core.Contracts.Product;

namespace WeVibe.Core.Services.Abstractions.Features
{
    public interface IProductService
    {
        Task<ProductDto> AddProductAsync(CreateProductDto productDto);
        Task<ProductDto> UpdateProductAsync(int productId, UpdateProductDto updateProductDto);
        Task<string> DeleteProductAsync(int productId);
        Task<IEnumerable<ProductDto>> GetAllProductsAsync();
        Task<ProductDto> GetProductByIdAsync(int productId);
        Task<ProductDetailDto> GetProductDetailByIdAsync(int productId);
        Task<IEnumerable<ProductDto>> GetProductsByCategoryAsync(int categoryId);
        Task<List<ProductDto>> SearchProductsByNameAsync(string searchString);
    }
}
