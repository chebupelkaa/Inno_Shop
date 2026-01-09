using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UserService.Domain.Interfaces;
using UserService.Infrastructure.Data;
using UserService.Infrastructure.Repositories;
using UserService.Infrastructure.Services;
using UserService.Application.Interfaces;

namespace UserService.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<UserDbContext>(options =>
               options.UseSqlServer(configuration.GetConnectionString("UsersDb")));

            services.AddScoped<IUserRepository, UserRepository>()
                .AddScoped<ITokenService, TokenService>()
                .AddScoped<IEmailService, EmailService>();
            //services.AddScoped<IEmailService, EmailService>();
            //services.AddScoped<ITokenService, TokenService>();
            //services.AddScoped<IProductServiceClient, ProductServiceClient>();

            return services;
        }
    }
}
