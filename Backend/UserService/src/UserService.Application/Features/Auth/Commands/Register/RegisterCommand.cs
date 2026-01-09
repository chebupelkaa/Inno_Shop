using MediatR;
using UserService.Application.DTOs;

namespace UserService.Application.Features.Auth.Commands.Register
{
    public class RegisterCommand : IRequest<TokenResponseDTO>
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
