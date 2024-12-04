using Microsoft.AspNetCore.Identity;
using WeVibe.Core.Contracts.Auth;
using WeVibe.Core.Contracts.User;

namespace WeVibe.Core.Services.Abstractions.Features
{
    public interface IAuthService
    {
        Task<IdentityResult> RegisterAsync(RegisterDto registerDto);
        Task<string> LoginAsync(LoginDto loginDto);
    }
}
