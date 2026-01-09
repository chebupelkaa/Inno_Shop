using UserService.Domain.Entities;

namespace UserService.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<User> CreateAsync(User user);
        Task<List<User>> GetAllAsync();
        Task<User?> GetByIdAsync(int id);
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetByNameAsync(string userName);
        Task<User> UpdateAsync(User user);
        Task DeleteAsync(int id);
        Task SaveAsync();
    }
}
