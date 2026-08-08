using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using ProductService.Application.Common;
using ProductService.Application.DTOs;
using ProductService.Domain.Entities;
using ProductService.Domain.Interfaces;

namespace ProductService.Application.Features.Products.Commands.CreateProduct
{
    public class CreateProductCommandHandler(IProductRepository productRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        : IRequestHandler<CreateProductCommand, ProductDto>
    {
        public async Task<ProductDto> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var product = new Product
            {
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
                Availability = request.Availability,
                UserId = CurrentUser.GetUserId(httpContextAccessor),
                DateOfCreation = DateTime.UtcNow
            };

            await productRepository.CreateAsync(product);
            await productRepository.SaveAsync();

            return mapper.Map<ProductDto>(product);
        }
    }
}
