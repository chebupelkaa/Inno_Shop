using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Moq;
using ProductService.Application.Exceptions;
using ProductService.Application.Features.Products.Commands.DeleteProduct;
using ProductService.Application.Features.Products.Commands.UpdateProduct;
using ProductService.Application.Profiles;
using ProductService.Domain.Entities;
using ProductService.Domain.Interfaces;

namespace ProductService.UnitTests.Handlers
{
    public class ProductOwnershipHandlersTests
    {
        private static IHttpContextAccessor CreateHttpContextAccessor(int userId)
        {
            var identity = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString())
            }, "Test");

            return new HttpContextAccessor
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(identity)
                }
            };
        }

        private static IMapper CreateMapper()
        {
            return new MapperConfiguration(cfg => cfg.AddProfile<ProductProfile>()).CreateMapper();
        }

        [Fact]
        public async Task Update_foreign_product_should_throw_forbidden()
        {
            var product = new Product
            {
                Id = 1,
                Name = "Phone",
                Description = "desc",
                Price = 100,
                UserId = 10
            };

            var repository = new Mock<IProductRepository>();
            repository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(product);

            var handler = new UpdateProductCommandHandler(repository.Object, CreateMapper(), CreateHttpContextAccessor(20));

            await Assert.ThrowsAsync<ForbiddenException>(() => handler.Handle(new UpdateProductCommand
            {
                Id = 1,
                Name = "New",
                Description = "desc",
                Price = 120,
                Availability = true
            }, CancellationToken.None));
        }

        [Fact]
        public async Task Delete_foreign_product_should_throw_forbidden()
        {
            var product = new Product
            {
                Id = 1,
                Name = "Phone",
                Description = "desc",
                Price = 100,
                UserId = 10
            };

            var repository = new Mock<IProductRepository>();
            repository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(product);

            var handler = new DeleteProductCommandHandler(repository.Object, CreateHttpContextAccessor(20));

            await Assert.ThrowsAsync<ForbiddenException>(() =>
                handler.Handle(new DeleteProductCommand(1), CancellationToken.None));
        }

        [Fact]
        public async Task Delete_own_product_should_succeed()
        {
            var product = new Product
            {
                Id = 1,
                Name = "Phone",
                Description = "desc",
                Price = 100,
                UserId = 10
            };

            var repository = new Mock<IProductRepository>();
            repository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(product);

            var handler = new DeleteProductCommandHandler(repository.Object, CreateHttpContextAccessor(10));
            await handler.Handle(new DeleteProductCommand(1), CancellationToken.None);

            repository.Verify(r => r.DeleteAsync(1), Times.Once);
            repository.Verify(r => r.SaveAsync(), Times.Once);
        }
    }
}
