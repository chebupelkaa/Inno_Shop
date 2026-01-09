using MediatR;
using UserService.Application.DTOs;

namespace UserService.Application.Features.Users.Queries.GetUsers
{
    public record GetUsersQuery : IRequest<List<UserDTO>>;
}
