using WeVibe.Core.Domain.Entities;

namespace WeVibe.Core.Domain.Repositories
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        Task<IEnumerable<Product>> GetAllWithCategoryAsync();
        Task<Product> GetProductDetailAsync(int id);
        Task<Product> GetProductByIdAsync(int id);
    }
}
