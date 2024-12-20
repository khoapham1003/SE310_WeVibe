using WeVibe.Core.Domain.Entities;

namespace WeVibe.Core.Domain.Repositories
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        Task<IEnumerable<Order>> GetOrdersWithTransactionsByUserIdAsync(string userId);
    }
}
