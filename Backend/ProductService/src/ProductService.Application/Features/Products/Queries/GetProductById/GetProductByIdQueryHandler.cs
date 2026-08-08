using AutoMapper;
using MediatR;
using ProductService.Application.DTOs;
using ProductService.Application.Exceptions;
using ProductService.Domain.Interfaces;

namespace ProductService.Application.Features.Products.Queries.GetProductById
{
    public class GetProductByIdQueryHandler(IProductRepository productRepository, IMapper mapper)
        : IRequestHandler<GetProductByIdQuery, ProductDto>
    {
        public async Task<ProductDto> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            var product = await productRepository.GetByIdAsync(request.Id);
            if (product == null)
            {
                throw new NotFoundException(typeof(ProductDto), request.Id);
            }

            return mapper.Map<ProductDto>(product);
        }
    }
}
