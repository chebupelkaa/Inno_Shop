using MediatR;

namespace UserService.Application.Features.Auth.Commands.ConfirmEmail
{
    public class ConfirmEmailCommand:IRequest<Unit>
    {
        public string Token { get; set; }
        public string Email { get; set; }
    }
}
