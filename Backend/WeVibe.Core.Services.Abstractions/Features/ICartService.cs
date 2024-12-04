using WeVibe.Core.Contracts.Cart;

namespace WeVibe.Core.Services.Abstractions.Features
{
    public interface ICartService
    {
        Task<CartDto> GetCartByUserIdAsync(string userId);
        Task<CartDto> AddToCartAsync(AddToCartDto addToCartDto);
        Task<bool> UpdateCartItemAsync(int cartItemId, UpdateCartItemDto updateDto);
        Task<bool> RemoveCartItemAsync(int cartItemId);
    }
}
