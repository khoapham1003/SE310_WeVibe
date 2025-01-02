using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using WeVibe.Core.Contracts.Auth;
using WeVibe.Core.Contracts.User;
using WeVibe.Core.Services.Abstractions.Features;

namespace WeVibe.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthService authService, IConfiguration configuration, ILogger<AuthController> logger)
        {
            _authService = authService;
            _configuration = configuration;
            _logger = logger;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            var result = await _authService.RegisterAsync(registerDto);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok(new { message = "Registration successful" });
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            try
            {
                var token = await _authService.LoginAsync(loginDto);
                return Ok(new { Token = token });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }
        [HttpPost("change-password")]
        [SwaggerOperation(Summary = "User change their password", Description = "User must login")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto dto)
        {
            var userId = dto.UserId;
            await _authService.ChangePasswordAsync(userId, dto.CurrentPassword, dto.NewPassword);
            return Ok("Password changed successfully.");
        }

        [HttpPost("generate-reset-password-token")]
        [SwaggerOperation(Summary = "Use to get token for input email", Description = "")]
        public async Task<IActionResult> GenerateResetPasswordToken([FromBody] ForgotPasswordDto dto)
        {
            var token = await _authService.GenerateResetPasswordTokenAsync(dto.Email);
            return Ok(new { Token = token });
        }

        [HttpPost("reset-password")]
        [SwaggerOperation(Summary = "Use to reset password when having reset token", Description = "")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto dto)
        {
            if (dto == null)
            {
                return BadRequest("Invalid data provided.");
            }

            if (string.IsNullOrEmpty(dto.Email) || string.IsNullOrEmpty(dto.Token) || string.IsNullOrEmpty(dto.NewPassword))
            {
                return BadRequest("Email, Token, and NewPassword are required.");
            }

            try
            {               
                await _authService.ResetPasswordAsync(dto.Email, dto.Token, dto.NewPassword);

                return Ok(new { message = "Password reset successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error resetting password: {ex.Message}");

                return StatusCode(500, "An unexpected error occurred.");
            }
        }
        [HttpPost("request-password-reset")]
        [SwaggerOperation(Summary = "Send reset link to email", Description = "")]
        public async Task<IActionResult> RequestPasswordReset([FromBody] ForgotPasswordDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Email))
            {
                return BadRequest("Email is required.");
            }

            try
            {
                var resetUrlBase = _configuration["Frontend:ResetPasswordUrl"];
                if (string.IsNullOrWhiteSpace(resetUrlBase))
                {
                    throw new Exception("Reset password URL is not configured.");
                }

                await _authService.SendResetPasswordEmailAsync(dto.Email, resetUrlBase);
                return Ok("Password reset email sent successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send password reset email for {Email}", dto.Email);
                return StatusCode(500, "An error occurred while sending the password reset email.");
            }
        }

    }
}
