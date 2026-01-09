using MediatR;

namespace UserService.Application.Features.Users.Commands.ChangeUserStatus
{
    public class ChangeUserStatusCommand : IRequest<bool>
    {
        public int UserId { get; set; }
        public bool IsActive { get; set; }
    }
}
