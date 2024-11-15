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
                .Include(p => p.Category)
                .ToListAsync();
        }
    }
}
