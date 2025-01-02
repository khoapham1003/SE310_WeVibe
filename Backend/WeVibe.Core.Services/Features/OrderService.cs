using AutoMapper;
using WeVibe.Core.Contracts.Order;
using WeVibe.Core.Contracts.Transaction;
using WeVibe.Core.Domain.Entities;
using WeVibe.Core.Domain.Repositories;
using WeVibe.Core.Services.Abstractions.Features;

namespace WeVibe.Core.Services.Features
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICartRepository _cartRepository;
        private readonly IMapper _mapper;

        public OrderService(IOrderRepository orderRepository, ICartRepository cartRepository, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _cartRepository = cartRepository;
            _mapper = mapper;
        }
        public async Task<OrderDto> GetOrderByIdAsync(int orderId)
        {
            var order = await _orderRepository.GetOrderWithTransactionAndItemsAsync(orderId);

            var orderDetailDto = new OrderDto
            {
                OrderId = order.OrderId,
                TotalAmount = order.TotalAmount,
                Status = order.Status,
                AddressValue = order.AddressValue,
                RecipientName = order.RecipientName,
                OrderItems = order.OrderItems.Select(oi => new OrderItemDto
                {
                    OrderItemId = oi.OrderItemId,
                    ProductId = oi.ProductId,
                    Quantity = oi.Quantity,
                    UnitPrice = oi.UnitPrice,
                    ProductVariantId = oi.ProductVariantId,
                    ProductName = oi.Product.Name,
                    SizeName = oi.ProductVariant.Size.Name,
                    ColorName = oi.ProductVariant.Color.Name
                }).ToList(),
            };

            return orderDetailDto;
        }
        public async Task<OrderDto> CreateOrderAsync(string userId)
        {
            var cart = await _cartRepository.GetCartWithItemsByUserIdAsync(userId);

            if (cart == null || !cart.CartItems.Any())
            {
                throw new Exception("Cart is empty or not found.");
            }

            decimal totalAmount = 0;
            foreach (var cartItem in cart.CartItems)
            {
                totalAmount += cartItem.UnitPrice * cartItem.Quantity;
            }

            var order = new Order
            {
                TotalAmount = totalAmount,
                Status = "Pending",
                UserId = userId,
                AddressValue = string.Empty, 
                RecipientName = "Unknown Recipient",
                OrderItems = cart.CartItems.Select(ci => new OrderItem
                {
                    ProductId = ci.ProductVariant.ProductId,
                    Quantity = ci.Quantity,
                    UnitPrice = ci.ProductVariant.Price,
                    ProductVariantId = ci.ProductVariantId
                }).ToList()
            };

            await _orderRepository.AddAsync(order);

            cart.CartItems.Clear();
            await _cartRepository.UpdateAsync(cart);

            var orderDto = new OrderDto
            {
                OrderId = order.OrderId,
                TotalAmount = order.TotalAmount,
                Status = order.Status,
                UserId = order.UserId,
                RecipientName = order.RecipientName,
                AddressValue = order.AddressValue,
                OrderItems = order.OrderItems.Select(oi => new OrderItemDto
                {
                    OrderItemId = oi.OrderItemId,
                    ProductId = oi.ProductId,
                    Quantity = oi.Quantity,
                    UnitPrice = oi.UnitPrice,
                    
                    ProductVariantId = oi.ProductVariantId,
                    ProductName = oi.Product.Name,
                    SizeName = oi.ProductVariant.Size.Name,
                    ColorName = oi.ProductVariant.Color.Name 
                }).ToList()
            };

            return orderDto;
        }
        public async Task<IEnumerable<OrderHistoryDto>> GetOrderHistoryAsync(string userId)
        {
            var orders = await _orderRepository.GetOrdersWithTransactionsByUserIdAsync(userId);

            var orderHistoryDtos = _mapper.Map<IEnumerable<OrderHistoryDto>>(orders);

            return orderHistoryDtos;
        }
        public async Task<OrderDto> GetOrderByIdAsync(int orderId)
        {
            var order = await _orderRepository.GetOrderWithTransactionAndItemsAsync(orderId);
            if (order == null)
            {
                throw new Exception("Order not found.");
            }

            var orderDetailDto = _mapper.Map<OrderDto>(order);

            return orderDetailDto;
            //var orderDetailDto = new OrderDto
            //{
            //    OrderId = order.OrderId,
            //    TotalAmount = order.TotalAmount,
            //    Status = order.Status,
            //    AddressValue = order.AddressValue,
            //    RecipientName = order.RecipientName,
            //    OrderItems = order.OrderItems.Select(oi => new OrderItemDto
            //    {
            //        OrderItemId = oi.OrderItemId,
            //        ProductId = oi.ProductId,
            //        Quantity = oi.Quantity,
            //        UnitPrice = oi.UnitPrice,
            //        ProductVariantId = oi.ProductVariantId,
            //        ProductName = oi.Product.Name,
            //        SizeName = oi.ProductVariant.Size.Name,
            //        ColorName = oi.ProductVariant.Color.Name
            //    }).ToList(),
            //};
        }

    }
}
