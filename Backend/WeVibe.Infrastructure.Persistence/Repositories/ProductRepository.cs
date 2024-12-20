using Microsoft.EntityFrameworkCore;
using WeVibe.Core.Domain.Entities;
using WeVibe.Core.Domain.Repositories;
using WeVibe.Infrastructure.Persistence.DataContext;

namespace WeVibe.Infrastructure.Persistence.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(ApplicationDbContext context) : base(context)
        {
        }
        public async Task<IEnumerable<Product>> GetAllWithCategoryAsync()
        {
            return await _context.Products
                .Include(p => p.Images)
                .Include(p => p.Category)
                .ToListAsync();
        }
        public async Task<Product> GetProductDetailAsync(int id)
        {
            return await _context.Set<Product>()
                .Include(p => p.Images)
                .Include(p => p.Category)
                .Include(p => p.ProductVariants)
                    .ThenInclude(pv => pv.Size)
                .Include(p => p.ProductVariants)
                    .ThenInclude(pv => pv.Color)
                .FirstOrDefaultAsync(p => p.ProductId == id);
        }
        public async Task<Product> GetProductByIdAsync(int id)
        {
            return await _context.Set<Product>()
                .Include(p => p.Images)
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.ProductId == id);
        }

        public async Task<IEnumerable<Product>> GetProductsByCategoryAsync(int categoryId)
        {
            return await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Images)
                .Where(p => p.CategoryId == categoryId)
                .ToListAsync();
        }
    }
}
