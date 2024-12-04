using WeVibe.Core.Domain.Entities;

namespace WeVibe.Core.Domain.Repositories
{
    public interface ICartRepository : IGenericRepository<Cart>
    {
        Task<Cart> GetCartByUserIdAsync(string userId);
        Task<Cart> GetCartWithItemsByUserIdAsync(string userId);
        Task<CartItem> GetCartItemByIdAsync(int cartItemId);
        Task DeleteCartItemAsync(int cartItemId);
    }
}
