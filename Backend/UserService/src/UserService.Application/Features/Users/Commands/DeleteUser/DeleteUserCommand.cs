using MediatR;

namespace UserService.Application.Features.Users.Commands.DeleteUser
{
    public record DeleteUserCommand(int id) : IRequest<bool>;

}
