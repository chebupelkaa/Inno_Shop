using ProductService.Domain.Entities;

namespace ProductService.Domain.Interfaces
{
    public interface IProductRepository
    {
            Task<Product> CreateAsync(Product product);
            Task<List<Product>> GetAllAsync();
            Task<Product?> GetByIdAsync(int id);
            Task<Product> UpdateAsync(Product product);
            Task DeleteAsync(int id);
            Task SaveAsync();
    }
}
