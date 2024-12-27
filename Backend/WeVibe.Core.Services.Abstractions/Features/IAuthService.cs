using Microsoft.AspNetCore.Identity;
using WeVibe.Core.Contracts.Auth;
using WeVibe.Core.Contracts.User;

namespace WeVibe.Core.Services.Abstractions.Features
{
    public interface IAuthService
    {
        Task<IdentityResult> RegisterAsync(RegisterDto registerDto);
        Task<string> LoginAsync(LoginDto loginDto);
        Task<string> GenerateResetPasswordTokenAsync(string email);
        Task ResetPasswordAsync(string email, string token, string newPassword);
        Task ChangePasswordAsync(string userId, string currentPassword, string newPassword);
        Task SendResetPasswordEmailAsync(string email, string resetUrlBase);
        Task<string> AssignRolesAsync(string userId, List<string> roles);
    }
}
