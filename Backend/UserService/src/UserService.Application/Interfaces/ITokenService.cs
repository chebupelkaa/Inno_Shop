using System.Security.Claims;
using UserService.Domain.Entities;

namespace UserService.Application.Interfaces
{
    public interface ITokenService
    {
        string GenerateAccessToken(User user);
        string GenerateRefreshToken();

        ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);

        public string GenerateSecureToken(int length = 32);

    }
}

//        Task<string> GenerateAndSaveRefreshToken(User user);
////Task<User?> ValidateRefreshTokenAsync(int userId, string refreshToken);
//ClaimsPrincipal? ValidateToken(string token);

