using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using UserService.API.Controllers;
using UserService.Application.Interfaces;
using UserService.Infrastructure.Data;

namespace UserService.IntegrationTests
{
    public class UserServiceWebApplicationFactory : WebApplicationFactory<AuthController>
    {
        private readonly string _databaseName = $"UsersTests_{Guid.NewGuid()}";

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Development");

            builder.ConfigureAppConfiguration((_, config) =>
            {
                config.AddInMemoryCollection(new Dictionary<string, string?>
                {
                    ["ConnectionStrings:UsersDb"] = "Server=(localdb)\\mssqllocaldb;Database=UsersTests;Trusted_Connection=True;",
                    ["Jwt:Secret"] = "DEV_ONLY_InnoShop_JwtSecret_Min32Chars!",
                    ["Jwt:Issuer"] = "UserService",
                    ["Jwt:Audience"] = "UserServiceClients",
                    ["Jwt:ExpiryMinutes"] = "60",
                    ["EmailConfiguration:From"] = "noreply@innoshop.local",
                    ["EmailConfiguration:SmtpServer"] = "localhost",
                    ["EmailConfiguration:Port"] = "1025",
                    ["AppUrl"] = "http://localhost"
                });
            });

            builder.ConfigureTestServices(services =>
            {
                RemoveDbContext<UserDbContext>(services);

                services.AddDbContext<UserDbContext>(options =>
                    options.UseInMemoryDatabase(_databaseName));

                services.RemoveAll<IEmailService>();
                services.AddSingleton(Mock.Of<IEmailService>());
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
