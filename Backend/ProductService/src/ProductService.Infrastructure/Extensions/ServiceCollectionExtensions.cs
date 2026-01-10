using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProductService.Domain.Interfaces;
using ProductService.Infrastructure.Data;
using ProductService.Infrastructure.Repositories;

namespace ProductService.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services,
           IConfiguration configuration)
        {
            services.AddDbContext<ProductDbContext>(options =>
               options.UseSqlServer(configuration.GetConnectionString("ProductsDb")));

            services.AddScoped<IProductRepository, ProductRepository>();
            //services.AddScoped<IEmailService, EmailService>();
            //services.AddScoped<ITokenService, TokenService>();
            //services.AddScoped<IProductServiceClient, ProductServiceClient>();

            return services;
        }
    }
}
