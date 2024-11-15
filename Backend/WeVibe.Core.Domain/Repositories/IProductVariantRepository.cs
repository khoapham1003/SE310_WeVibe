using WeVibe.Core.Domain.Entities;

namespace WeVibe.Core.Domain.Repositories
{
    public interface IProductVariantRepository : IGenericRepository<ProductVariant>
    {
        Task<ProductVariant> GetProductVariantByIdAsync(int id);
        Task<IEnumerable<ProductVariant>> GetAllProductVariantsAsync();
    }
}
