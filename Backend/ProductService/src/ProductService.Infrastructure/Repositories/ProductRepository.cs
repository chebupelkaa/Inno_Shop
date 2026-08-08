using Microsoft.EntityFrameworkCore;
using ProductService.Domain.Entities;
using ProductService.Domain.Interfaces;
using ProductService.Infrastructure.Data;

namespace ProductService.Infrastructure.Repositories
{
    public class ProductRepository(ProductDbContext context) : IProductRepository
    {
        public async Task<Product> CreateAsync(Product product)
        {
            await context.Products.AddAsync(product);
            return product;
        }

        public async Task<List<Product>> GetAllAsync()
        {
            return await context.Products.ToListAsync();
        }

        public async Task<List<Product>> GetFilteredAsync(string? name, decimal? minPrice, decimal? maxPrice, 
            bool? availability, int? userId, DateTime? createdFrom, DateTime? createdTo)
        {
            var query = context.Products.AsQueryable();

            if (!string.IsNullOrWhiteSpace(name))
            {
                query = query.Where(p => p.Name.Contains(name));
            }

            if (minPrice.HasValue)
            {
                query = query.Where(p => p.Price >= minPrice.Value);
            }

            if (maxPrice.HasValue)
            {
                query = query.Where(p => p.Price <= maxPrice.Value);
            }

            if (availability.HasValue)
            {
                query = query.Where(p => p.Availability == availability.Value);
            }

            if (userId.HasValue)
            {
                query = query.Where(p => p.UserId == userId.Value);
            }

            if (createdFrom.HasValue)
            {
                query = query.Where(p => p.DateOfCreation >= createdFrom.Value);
            }

            if (createdTo.HasValue)
            {
                query = query.Where(p => p.DateOfCreation <= createdTo.Value);
            }

            return await query.OrderByDescending(p => p.DateOfCreation).ToListAsync();
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            return await context.Products.FirstOrDefaultAsync(p => p.Id == id);
        }

        public Task<Product> UpdateAsync(Product product)
        {
            context.Products.Update(product);
            return Task.FromResult(product);
        }

        public async Task DeleteAsync(int id)
        {
            var product = await GetByIdAsync(id);
            if (product != null)
            {
                context.Products.Remove(product);
            }
        }

        public async Task SaveAsync()
        {
            await context.SaveChangesAsync();
        }
    }
}
