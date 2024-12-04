using WeVibe.Core.Domain.Entities;
using WeVibe.Core.Domain.Repositories;
using WeVibe.Infrastructure.Persistence.DataContext;

namespace WeVibe.Infrastructure.Persistence.Repositories
{
    public class DiscountRepository : GenericRepository<Discount>, IDiscountRepository
    {
        public DiscountRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
