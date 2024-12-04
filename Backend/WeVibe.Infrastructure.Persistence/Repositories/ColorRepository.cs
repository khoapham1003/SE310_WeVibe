using WeVibe.Core.Domain.Entities;
using WeVibe.Core.Domain.Repositories;
using WeVibe.Infrastructure.Persistence.DataContext;

namespace WeVibe.Infrastructure.Persistence.Repositories
{
    public class ColorRepository : GenericRepository<Color>, IColorRepository
    {
        public ColorRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
