using MediatR;
using UserService.Application.DTOs;

namespace UserService.Application.Features.Users.Commands.UpdateUser
{
    //public record UpdateUserCommand(int id, string name, string Email, bool isActive, bool IsEmailConfirmed) : IRequest<UserDTO>;

    //public class UpdateUserCommand : IRequest<bool>
    //{
    //    public int UserId { get; set; }
    //}
    public record UpdateUserCommand(UserDTO newUser) : IRequest<UserDTO>;

}
