using Microsoft.EntityFrameworkCore;
using WeVibe.Core.Domain.Entities;
using WeVibe.Core.Domain.Repositories;
using WeVibe.Infrastructure.Persistence.DataContext;

namespace WeVibe.Infrastructure.Persistence.Repositories
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        private readonly ApplicationDbContext _context;
        public OrderRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Order>> GetOrdersWithTransactionsByUserIdAsync(string userId)
        {
            return await _context.Orders
                .Where(o => o.UserId == userId)
                .Include(o => o.OrderItems) 
                    .ThenInclude(oi => oi.Product)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.ProductVariant)  // Include ProductVariant within OrderItems
                        .ThenInclude(pv => pv.Size)  // Include Size within ProductVariant
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.ProductVariant)  // Include ProductVariant within OrderItems
                        .ThenInclude(pv => pv.Color)
                .Include(o => o.Transaction)
                .ToListAsync();
        }

        // Method to get a specific order with its transaction and order items
        public async Task<Order> GetOrderWithTransactionAndItemsAsync(int orderId)
        {
            return await _context.Orders
                .Where(o => o.OrderId == orderId)
                .Include(o => o.OrderItems)  // Include OrderItems
                    .ThenInclude(oi => oi.Product)  // Include Product within OrderItems
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.ProductVariant)  // Include ProductVariant within OrderItems
                        .ThenInclude(pv => pv.Size)  // Include Size within ProductVariant
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.ProductVariant)  // Include ProductVariant within OrderItems
                        .ThenInclude(pv => pv.Color) // Include ProductVariant within OrderItems
                .Include(o => o.Transaction)  // Include Transaction
                .FirstOrDefaultAsync();
        }
    }
}
