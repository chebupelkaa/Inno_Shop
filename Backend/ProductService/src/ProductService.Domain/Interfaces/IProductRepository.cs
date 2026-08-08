using ProductService.Domain.Entities;

namespace ProductService.Domain.Interfaces
{
    public interface IProductRepository
    {
        Task<Product> CreateAsync(Product product);
        Task<List<Product>> GetAllAsync();
        Task<List<Product>> GetFilteredAsync(string? name, decimal? minPrice, decimal? maxPrice, bool? availability, 
            int? userId, DateTime? createdFrom, DateTime? createdTo);
        Task<Product?> GetByIdAsync(int id);
        Task<Product> UpdateAsync(Product product);
        Task DeleteAsync(int id);
        Task SaveAsync();
    }
}
