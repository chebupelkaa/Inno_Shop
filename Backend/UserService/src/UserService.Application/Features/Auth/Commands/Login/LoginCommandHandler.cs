using MediatR;
using Microsoft.AspNetCore.Http;
using UserService.Application.DTOs;
using UserService.Application.Interfaces;
using UserService.Domain.Interfaces;

namespace UserService.Application.Features.Auth.Commands.Login
{
    public class LoginCommandHandler(IUserRepository userRepository,
        IHttpContextAccessor httpContextAccessor, IAuthenticationService authService)
        : IRequestHandler<LoginCommand, TokenResponseDTO>
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly IAuthenticationService _authService = authService;
        public async Task<TokenResponseDTO> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var currentUser = _httpContextAccessor.HttpContext?.User;
            if (currentUser?.Identity?.IsAuthenticated == true)
            {
                throw new InvalidOperationException("You are already logged in");
            }

            var user = await _userRepository.GetByEmailAsync(request.Email);
            if (user == null)
            {
                throw new UnauthorizedAccessException("Invalid credentials");
            }

            if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                throw new UnauthorizedAccessException("Invalid email or password");
            }

            if (!user.IsActive)
            {
                throw new InvalidOperationException("Account is deactivated");
            }

            return await _authService.GenerateAuthenticationAsync(user);
        }
    }
}
