using WeVibe.Core.Domain.Entities;
using WeVibe.Core.Domain.Repositories;
using WeVibe.Infrastructure.Persistence.DataContext;

namespace WeVibe.Infrastructure.Persistence.Repositories
{
    public class ProductImageRepository : GenericRepository<ProductImage>, IProductImageRepository
    {
        public ProductImageRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
