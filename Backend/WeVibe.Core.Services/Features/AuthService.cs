using Microsoft.AspNetCore.Identity;
using System.Text.RegularExpressions;
using WeVibe.Core.Contracts.Auth;
using WeVibe.Core.Contracts.User;
using WeVibe.Core.Domain.Entities;
using WeVibe.Core.Services.Abstractions.Features;

namespace WeVibe.Core.Services.Features
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly IEmailService _emailService;

        public AuthService(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ITokenService tokenService, IEmailService emailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _emailService = emailService;
        }
        public async Task<IdentityResult> RegisterAsync(RegisterDto registerDto)
        {
            if (!IsValidEmail(registerDto.Email))
            {
                throw new ArgumentException("The email format is invalid.");
            }

            if (registerDto.Password != registerDto.RePassword)
            {
                throw new ArgumentException("Passwords do not match.");
            }

            var user = new ApplicationUser
            {
                UserName = registerDto.Email,
                Email = registerDto.Email,
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                DateCreated = DateTime.Now
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);
            return result;
        }
        public async Task<string> LoginAsync(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);

            if (user == null || !await _userManager.CheckPasswordAsync(user, loginDto.Password))
            {
                throw new UnauthorizedAccessException("Invalid login attempt.");
            }

            var result = await _signInManager.PasswordSignInAsync(user, loginDto.Password, false, false);

            if (!result.Succeeded)
            {
                throw new UnauthorizedAccessException("Invalid login attempt.");
            }

            // Generate JWT Token
            var token = await _tokenService.GenerateAccessTokenAsync(user);

            return token;
        }
        private bool IsValidEmail(string email)
        {
            var emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
            return emailRegex.IsMatch(email);
        }

        public async Task<string> GenerateResetPasswordTokenAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                throw new Exception("User not found.");
            }

            return await _userManager.GeneratePasswordResetTokenAsync(user);
        }

        public async Task ResetPasswordAsync(string email, string token, string newPassword)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                throw new Exception("User not found.");
            }

            var result = await _userManager.ResetPasswordAsync(user, token, newPassword);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new Exception($"Password reset failed: {errors}");
            }
        }
        public async Task ChangePasswordAsync(string userId, string currentPassword, string newPassword)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new Exception("User not found.");
            }

            var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new Exception($"Password change failed: {errors}");
            }
        }
        public async Task SendResetPasswordEmailAsync(string email, string resetUrlBase)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                throw new Exception("User not found.");
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            var resetUrl = $"{resetUrlBase}?email={Uri.EscapeDataString(email)}&token={Uri.EscapeDataString(token)}";

            var emailBody = $@"
                                <h1>Reset Your Password</h1>
                                <p>Click the link below to reset your password:</p>
                                <a href='{resetUrl}'>Reset Password</a>";

            await _emailService.SendEmailAsync(email, "Password Reset Request", emailBody);
        }

    }
}
