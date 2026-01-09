using AutoMapper;
using UserService.Application.DTOs;
using UserService.Application.Interfaces;
using UserService.Domain.Entities;
using UserService.Domain.Interfaces;

namespace UserService.Application.Services
{
    public class AuthenticationService(
      IUserRepository userRepository,
      ITokenService tokenService,
      IMapper mapper) : IAuthenticationService
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly ITokenService _tokenService = tokenService;
        private readonly IMapper _mapper = mapper;
        private const int REFRESH_TOKEN_EXPIRY_DAYS = 7;

        public async Task<TokenResponseDTO> GenerateAuthenticationAsync(User user)
        {
            var refreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(REFRESH_TOKEN_EXPIRY_DAYS);

            await _userRepository.UpdateAsync(user);
            await _userRepository.SaveAsync();

            return new TokenResponseDTO
            {
                AccessToken = _tokenService.GenerateAccessToken(user),
                RefreshToken = refreshToken,
                User = _mapper.Map<UserDTO>(user),
            };
        }

        public async Task RevokeAuthenticationAsync(User user)
        {
            user.RefreshToken = null;
            user.RefreshTokenExpiryTime = null;

            await _userRepository.UpdateAsync(user);
            await _userRepository.SaveAsync();
        }
    }
}
