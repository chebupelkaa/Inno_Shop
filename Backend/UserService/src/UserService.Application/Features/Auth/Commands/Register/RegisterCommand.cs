using MediatR;

namespace UserService.Application.Features.Auth.Commands.Register
{
    public class RegisterCommand : IRequest<Unit>
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
