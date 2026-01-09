using UserService.Application.DTOs;
using UserService.Domain.Entities;

namespace UserService.Application.Interfaces
{
    public interface IAuthenticationService
    {
        Task<TokenResponseDTO> GenerateAuthenticationAsync(User user);
        Task RevokeAuthenticationAsync(User user);
    }
}
