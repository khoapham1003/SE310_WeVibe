using Microsoft.EntityFrameworkCore;
using WeVibe.Core.Domain.Entities;
using WeVibe.Core.Domain.Repositories;
using WeVibe.Infrastructure.Persistence.DataContext;

namespace WeVibe.Infrastructure.Persistence.Repositories
{
    public class ProductVariantRepository : GenericRepository<ProductVariant>, IProductVariantRepository
    {
        public ProductVariantRepository(ApplicationDbContext context) : base(context)
        {
        }
        public async Task<ProductVariant> GetProductVariantByIdAsync(int id)
        {
            return await _context.ProductVariants
                .Include(pv => pv.Product)
                    .ThenInclude(pv => pv.Images)
                .Include(pv => pv.Size)
                .Include(pv => pv.Color)
                .FirstOrDefaultAsync(pv => pv.ProductVariantId == id);
        }
        public async Task<IEnumerable<ProductVariant>> GetAllProductVariantsAsync()
        {
            return await _context.ProductVariants
                .Include(pv => pv.Product)
                .Include(pv => pv.Size)
                .Include(pv => pv.Color)
                .ToListAsync();
        }
    }
}
