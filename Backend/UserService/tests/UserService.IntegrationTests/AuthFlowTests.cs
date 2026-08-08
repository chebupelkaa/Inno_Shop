using System.Net;
using System.Net.Http.Json;
using Microsoft.Extensions.DependencyInjection;
using UserService.Domain.Entities;
using UserService.Domain.Enums;
using UserService.Infrastructure.Data;

namespace UserService.IntegrationTests
{
    public class AuthFlowTests : IClassFixture<UserServiceWebApplicationFactory>
    {
        private readonly UserServiceWebApplicationFactory _factory;

        public AuthFlowTests(UserServiceWebApplicationFactory factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task Login_with_confirmed_user_should_return_tokens()
        {
            const string email = "confirmed@example.com";
            const string password = "Password1!";

            using (var scope = _factory.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<UserDbContext>();
                db.Users.Add(new User
                {
                    Name = "Confirmed",
                    Email = email,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
                    Role = UserRole.User,
                    IsActive = true,
                    IsEmailConfirmed = true
                });
                await db.SaveChangesAsync();
            }

            var client = _factory.CreateClient();
            var response = await client.PostAsJsonAsync("/api/auth/login", new
            {
                Email = email,
                Password = password
            });

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var payload = await response.Content.ReadFromJsonAsync<LoginResponse>();
            Assert.False(string.IsNullOrWhiteSpace(payload?.AccessToken));
        }

        [Fact]
        public async Task ForgotPassword_unknown_email_should_return_ok()
        {
            var client = _factory.CreateClient();
            var response = await client.PostAsJsonAsync("/api/auth/forgot-password", new
            {
                Email = "does-not-exist@example.com"
            });

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        private sealed class LoginResponse
        {
            public string AccessToken { get; set; } = string.Empty;
        }
    }
}
