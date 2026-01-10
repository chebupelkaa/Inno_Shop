
using Microsoft.EntityFrameworkCore;
using ProductService.Domain.Entities;

namespace ProductService.Infrastructure.Data
{
    public class ProductDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public ProductDbContext(DbContextOptions<ProductDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(u => u.Id);

                entity.HasIndex(p => p.UserId); 
                entity.HasIndex(p => p.Name); 
                entity.HasIndex(p => p.Price);
                entity.HasIndex(p => p.Availability); 
                entity.HasIndex(p => p.DateOfCreation);

                entity.Property(p => p.Name).IsRequired();

                entity.Property(p => p.Price).HasColumnType("decimal(18,2)"); 

                entity.Property(p => p.DateOfCreation).HasDefaultValueSql("GETUTCDATE()");
            });
        }
    }
}
