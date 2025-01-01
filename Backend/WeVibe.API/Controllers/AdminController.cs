using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using WeVibe.Core.Contracts.User;
using WeVibe.Core.Services.Abstractions.Features;
using WeVibe.Core.Services.Features;

namespace WeVibe.API.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : Controller
    {
        private readonly IAuthService _authService;
        private readonly IUserService _userService;
        public AdminController(IAuthService authService, IUserService userService) 
        { 
            _authService = authService;
            _userService = userService;
        }
        //[HttpPost("{userId}/roles")]
        //public async Task<IActionResult> AssignRoles(string userId, [FromBody] List<string> roles)
        //{
        //    var result = await _authService.AssignRolesAsync(userId, roles);

        //    if (result == "User not found")
        //    {
        //        return NotFound(result);
        //    }

        //    if (result.StartsWith("Failed"))
        //    {
        //        return BadRequest(result);
        //    }

        //    return Ok(result);
        //}
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

        // 4. Update User
        //[HttpPut("{id}")]
        //[SwaggerOperation(Summary = "Update user (email, roles)", Description = "")]
        //public async Task<IActionResult> UpdateUser(string id, [FromBody] UpdateUserDto dto)
        //{
        //    var result = await _userService.UpdateUserAsync(id, dto);
        //    if (!result.Success)
        //    {
        //        return BadRequest(result.Errors);
        //    }
        //    return Ok("User updated successfully.");
        //}

        // 5. Delete User
        [HttpDelete("{id}")]
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

    }
}
