using AutoMapper;
using MediatR;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using UserService.Application.DTOs;
using UserService.Application.Interfaces;
using UserService.Domain.Interfaces;

namespace UserService.Application.Features.Auth.Commands.RefreshToken
{
    public class RefreshTokenCommandHandler(IUserRepository userRepository, ITokenService tokenService, IAuthenticationService authService) : IRequestHandler<RefreshTokenCommand, TokenResponseDTO>
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly ITokenService _tokenService = tokenService;
        private readonly IAuthenticationService _authService= authService;
        public async Task<TokenResponseDTO> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var principal = _tokenService.GetPrincipalFromExpiredToken(request.AccessToken);
            if (principal == null)
            {
                throw new SecurityTokenException("Invalid access token");
            }

            var userIdClaim = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
            {
                throw new SecurityTokenException("Invalid token claims");
            }
            var userId = int.Parse(userIdClaim);

            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null || user.RefreshToken != request.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                throw new SecurityTokenException("Invalid or expired refresh token");
            }

            return await _authService.GenerateAuthenticationAsync(user);
        }
    }
}
