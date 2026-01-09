using MediatR;

namespace UserService.Application.Features.Auth.Commands.SendEmailConfirmation
{
    public  class SendEmailConfirmationCommand:IRequest<Unit>
    {
        public string Email { get; set; }
    }
}
