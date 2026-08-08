using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using ProductService.Application.Common;
using ProductService.Application.DTOs;
using ProductService.Application.Exceptions;
using ProductService.Domain.Interfaces;

namespace ProductService.Application.Features.Products.Commands.UpdateProduct
{
    public class UpdateProductCommandHandler(IProductRepository productRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        : IRequestHandler<UpdateProductCommand, ProductDto>
    {
        public async Task<ProductDto> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var product = await productRepository.GetByIdAsync(request.Id);
            if (product == null)
            {
                throw new NotFoundException(typeof(ProductDto), request.Id);
            }

            if (product.UserId != CurrentUser.GetUserId(httpContextAccessor))
            {
                throw new ForbiddenException("You can only update your own products");
            }

            product.Name = request.Name;
            product.Description = request.Description;
            product.Price = request.Price;
            product.Availability = request.Availability;

            await productRepository.UpdateAsync(product);
            await productRepository.SaveAsync();

            return mapper.Map<ProductDto>(product);
        }
    }
}
