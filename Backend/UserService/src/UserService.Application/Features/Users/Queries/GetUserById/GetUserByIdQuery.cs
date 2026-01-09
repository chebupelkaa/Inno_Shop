using MediatR;
using UserService.Application.DTOs;

namespace UserService.Application.Features.Users.Queries.GetUserById
{
    public record GetUserByIdQuery(int id) : IRequest<UserDTO>;
}
