using System.Security.Claims;
using WeVibe.Core.Domain.Entities;

namespace WeVibe.Core.Services.Abstractions.Features
{
    public interface ITokenService
    {
        Task<string> GenerateAccessTokenAsync(ApplicationUser user);
        Task<string> GenerateRefreshTokenAsync(ApplicationUser user);
        Task<ClaimsPrincipal> GetPrincipalFromExpiredTokenAsync(string token);
        Task<bool> ValidateTokenAsync(string token);
    }
}
