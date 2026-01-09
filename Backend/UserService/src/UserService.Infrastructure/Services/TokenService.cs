using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using UserService.Application.Interfaces;
using UserService.Domain.Entities;

namespace UserService.Infrastructure.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateAccessToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Role, user.Role.ToString()),
                new Claim("isActive", user.IsActive.ToString()),
                new Claim("IsEmailConfirmed", user.IsEmailConfirmed.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Secret"]!));
            var token = new JwtSecurityToken(
                 issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["Jwt:ExpiryMinutes"])),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        public ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Secret"]!);

            try
            {
                var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = validatedToken as JwtSecurityToken;
                if (jwtToken == null || !jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                    return null;

                return principal;
            }
            catch
            {
                return null;
            }
        }
        public string GenerateSecureToken(int length = 32)
        {
            if (length <= 0)
            {
                throw new ArgumentException("Token length must be positive", nameof(length));
            }

            using var rng = RandomNumberGenerator.Create();
            var bytes = new byte[length];
            rng.GetBytes(bytes);

            return Convert.ToBase64String(bytes)
                .Replace("+", "-")
                .Replace("/", "_")
                .Replace("=", "");
        }

    }
}


//public async Task<string> GenerateAndSaveRefreshToken(User user)
//{
//    var refreshToken = GenerateRefreshToken();
//    user.RefreshToken = refreshToken;
//    user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
//    await _userRepository.UpdateAsync(user);
//    await _userRepository.SaveAsync();
//    return refreshToken;
//}

//public ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)
//{
//    var tokenHandler = new JwtSecurityTokenHandler();
//    var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!);

//    try
//    {
//        var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
//        {
//            ValidateIssuerSigningKey = true,
//            IssuerSigningKey = new SymmetricSecurityKey(key),
//            ValidateIssuer = false,
//            ValidIssuer = _configuration["Jwt:Issuer"],
//            ValidateAudience = true,
//            ValidAudience = _configuration["Jwt:Audience"],
//            ValidateLifetime = false,
//            ClockSkew = TimeSpan.Zero
//        }, out SecurityToken validatedToken);

//        return principal;
//    }
//    catch
//    {
//        return null;
//    }
//}