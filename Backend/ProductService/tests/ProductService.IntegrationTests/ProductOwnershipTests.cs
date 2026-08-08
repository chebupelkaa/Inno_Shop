using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using ProductService.Application.DTOs;

namespace ProductService.IntegrationTests
{
    public class ProductOwnershipTests : IClassFixture<ProductServiceWebApplicationFactory>
    {
        private const string JwtSecret = "DEV_ONLY_InnoShop_JwtSecret_Min32Chars!";
        private readonly ProductServiceWebApplicationFactory _factory;

        public ProductOwnershipTests(ProductServiceWebApplicationFactory factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task Owner_can_create_product_and_stranger_gets_403_on_delete()
        {
            var ownerClient = _factory.CreateClient();
            ownerClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", CreateToken(userId: 1));

            var createResponse = await ownerClient.PostAsJsonAsync("/api/products", new
            {
                Name = "Laptop",
                Description = "Work laptop",
                Price = 1500m,
                Availability = true
            });

            Assert.Equal(HttpStatusCode.Created, createResponse.StatusCode);
            var created = await createResponse.Content.ReadFromJsonAsync<ProductDto>();
            Assert.NotNull(created);

            var strangerClient = _factory.CreateClient();
            strangerClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", CreateToken(userId: 2));

            var deleteResponse = await strangerClient.DeleteAsync($"/api/products/{created!.Id}");
            Assert.Equal(HttpStatusCode.Forbidden, deleteResponse.StatusCode);
        }

        private static string CreateToken(int userId)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtSecret));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "UserService",
                audience: "UserServiceClients",
                claims:
                [
                    new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                    new Claim(ClaimTypes.Email, $"user{userId}@example.com"),
                    new Claim(ClaimTypes.Role, "User")
                ],
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
