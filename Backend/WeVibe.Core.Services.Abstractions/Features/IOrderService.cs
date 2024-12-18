using WeVibe.Core.Contracts.Order;

namespace WeVibe.Core.Services.Abstractions.Features
{
    public interface IOrderService
    {
        Task<OrderDto> CreateOrderAsync(string userId, string address);
        Task<IEnumerable<OrderHistoryDto>> GetOrderHistoryAsync(string userId);
    }
}
