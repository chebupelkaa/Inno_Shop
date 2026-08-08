using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UserService.Application.Interfaces;
using UserService.Application.Options;
using UserService.Domain.Interfaces;
using UserService.Infrastructure.Data;
using UserService.Infrastructure.Repositories;
using UserService.Infrastructure.Services;

namespace UserService.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddOptions<ConnectionStringsOptions>()
                .Bind(configuration.GetSection(ConnectionStringsOptions.SectionName))
                .ValidateDataAnnotations()
                .ValidateOnStart();

            services.AddOptions<JwtOptions>()
                .Bind(configuration.GetSection(JwtOptions.SectionName))
                .ValidateDataAnnotations()
                .ValidateOnStart();

            services.AddOptions<EmailConfigurationOptions>()
                .Bind(configuration.GetSection(EmailConfigurationOptions.SectionName))
                .ValidateDataAnnotations()
                .ValidateOnStart();

            var connectionString = configuration.GetConnectionString("UsersDb");
            services.AddDbContext<UserDbContext>(options =>
               options.UseSqlServer(connectionString));

            services.AddScoped<IUserRepository, UserRepository>()
                .AddScoped<ITokenService, TokenService>()
                .AddScoped<IEmailService, EmailService>();

            return services;
        }
    }
}
