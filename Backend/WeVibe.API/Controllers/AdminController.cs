using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WeVibe.Core.Services.Abstractions.Features;

namespace WeVibe.API.Controllers
{
    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("api/admin")]
    public class AdminController : Controller
    {
        private readonly IAuthService _authService;
        public AdminController(IAuthService authService) 
        { 
            _authService = authService;
        }
        [HttpPost("{userId}/roles")]
        public async Task<IActionResult> AssignRoles(string userId, [FromBody] List<string> roles)
        {
            var result = await _authService.AssignRolesAsync(userId, roles);

            if (result == "User not found")
            {
                return NotFound(result);
            }

            if (result.StartsWith("Failed"))
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
    }
}
