using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using WeVibe.Core.Contracts.Order;
using WeVibe.Core.Services.Abstractions.Features;

namespace WeVibe.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }
        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDto createOrderDto)
        {
            try
            {
                var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized("User ID not found in token.");
                }

                var orderDto = await _orderService.CreateOrderAsync(userId, createOrderDto.Address);

                return Ok(new
                {
                    Status = "Success",
                    Message = "Order created successfully.",
                    OrderId = orderDto.OrderId,
                    OrderDetails = orderDto
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message, innerException = ex.InnerException?.Message });
            }
        }
        [HttpGet("orders-history")]
        [SwaggerOperation(Summary = "Get order history", Description = "Retrieve the user's order history including transaction details.")]
        [SwaggerResponse(200, "Order history retrieved successfully", typeof(IEnumerable<OrderHistoryDto>))]
        [SwaggerResponse(401, "Unauthorized - User not found in token.")]
        [SwaggerResponse(404, "No orders found for this user.")]
        public async Task<IActionResult> GetOrderHistory()
        {
            try
            {
                var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized("User ID not found in token.");
                }

                var orderHistory = await _orderService.GetOrderHistoryAsync(userId);

                if (orderHistory == null || !orderHistory.Any())
                {
                    return NotFound("No orders found for this user.");
                }

                return Ok(orderHistory);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
