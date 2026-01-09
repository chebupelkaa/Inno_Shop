using MediatR;
using UserService.Application.DTOs;

namespace UserService.Application.Features.Auth.Commands.Login
{
    public class LoginCommand : IRequest<TokenResponseDTO>
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
