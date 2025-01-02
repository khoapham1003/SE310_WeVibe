using WeVibe.Core.Contracts.Order;

namespace WeVibe.Core.Services.Abstractions.Features
{
    public interface IOrderService
    {
        Task<OrderDto> CreateOrderAsync(string userId);
        Task<IEnumerable<OrderHistoryDto>> GetOrderHistoryAsync(string userId);
        Task<OrderDto> GetOrderByIdAsync(int orderId);
        Task<List<OrderDto>> GetAllOrdersAsync();
        Task<OrderDto> UpdateOrderStatusAsync(int orderId, string status);
        Task DeleteOrderAsync(int orderId);
    }
}
