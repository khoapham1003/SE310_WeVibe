using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using WeVibe.Core.Contracts.Order;
using WeVibe.Core.Contracts.User;
using WeVibe.Core.Services.Abstractions.Features;
using WeVibe.Core.Services.Features;

namespace WeVibe.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : Controller
    {
        private readonly IAuthService _authService;
        private readonly IUserService _userService;
        private readonly IOrderService _orderService;
        public AdminController(IAuthService authService, IUserService userService, IOrderService orderService) 
        { 
            _authService = authService;
            _userService = userService;
            _orderService = orderService;
        }
        [HttpGet("/Get-All-Users")]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var users = await _userService.GetAllUsersAsync();

                return Ok(users);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // 2. Get User by ID
        [HttpGet("/Get-User/{id}")]
        [SwaggerOperation(Summary = "Get User by ID", Description = "")]
        public async Task<IActionResult> GetUserById(string id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound($"User with ID {id} not found.");
            }
            return Ok(user);
        }

        [HttpPost("/CreateUser")]
        [SwaggerOperation(Summary = "Admin create new user", Description = "")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserDto dto)
        {
            try
            {
                var userId = await _userService.CreateUserAsync(dto);

                return CreatedAtAction(nameof(GetUserById), new { id = userId }, userId);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // 5. Delete User
        [HttpDelete("/Delete-User/{id}")]
        [SwaggerOperation(Summary = "Delete user", Description = "")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var result = await _userService.DeleteUserAsync(id);
            if (!result.Success)
            {
                return BadRequest(result.Errors);
            }
            return Ok("User deleted successfully.");
        }

        // 6. Change User Role
        [HttpPost("{id}/roles")]
        [SwaggerOperation(Summary = "Assign roles for user", Description = "")]
        public async Task<IActionResult> ChangeUserRole(string id, [FromBody] List<string> roles)
        {
            var result = await _userService.ChangeUserRolesAsync(id, roles);
            if (!result.Success)
            {
                return BadRequest(result.Errors);
            }
            return Ok("Roles updated successfully.");
        }

        [HttpGet("Get-All-Orders")]
        [SwaggerOperation(Summary = "Get All Orders", Description = "")]
        public async Task<IActionResult> GetAllOrders()
        {
            try
            {
                var orders = await _orderService.GetAllOrdersAsync();
                return Ok(orders);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }            
        }
        [HttpPut("{orderId}/status")]
        [SwaggerOperation(Summary = "Update Order Status", Description = "")]
        public async Task<IActionResult> UpdateOrderStatus(int orderId, [FromBody] UpdateOrderStatusDto dto)
        {
            try
            {
                var updatedOrder = await _orderService.UpdateOrderStatusAsync(orderId, dto.Status);
                return Ok(updatedOrder);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
