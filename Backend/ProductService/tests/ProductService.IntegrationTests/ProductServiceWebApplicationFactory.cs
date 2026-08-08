using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProductService.API.Controllers;
using ProductService.Infrastructure.Data;

namespace ProductService.IntegrationTests
{
    public class ProductServiceWebApplicationFactory : WebApplicationFactory<ProductsController>
    {
        private readonly string _databaseName = $"ProductsTests_{Guid.NewGuid()}";

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Development");

            builder.ConfigureAppConfiguration((_, config) =>
            {
                config.AddInMemoryCollection(new Dictionary<string, string?>
                {
                    ["ConnectionStrings:ProductsDb"] = "Server=(localdb)\\mssqllocaldb;Database=ProductsTests;Trusted_Connection=True;",
                    ["Jwt:Secret"] = "DEV_ONLY_InnoShop_JwtSecret_Min32Chars!",
                    ["Jwt:Issuer"] = "UserService",
                    ["Jwt:Audience"] = "UserServiceClients",
                    ["Jwt:ExpiryMinutes"] = "60",
                    ["AppUrl"] = "http://localhost"
                });
            });

            builder.ConfigureTestServices(services =>
            {
                RemoveDbContext<ProductDbContext>(services);

                services.AddDbContext<ProductDbContext>(options =>
                    options.UseInMemoryDatabase(_databaseName));
            });
        }

        private static void RemoveDbContext<TContext>(IServiceCollection services) where TContext : DbContext
        {
            var descriptors = services.Where(d =>
                    d.ServiceType == typeof(TContext) ||
                    d.ServiceType == typeof(DbContextOptions) ||
                    d.ServiceType == typeof(DbContextOptions<TContext>) ||
                    d.ServiceType == typeof(IDbContextOptionsConfiguration<TContext>))
                .ToList();

            foreach (var descriptor in descriptors)
            {
                services.Remove(descriptor);
            }
        }
    }
}
