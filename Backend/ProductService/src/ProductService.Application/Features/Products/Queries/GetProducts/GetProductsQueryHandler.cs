using AutoMapper;
using MediatR;
using ProductService.Application.DTOs;
using ProductService.Domain.Interfaces;

namespace ProductService.Application.Features.Products.Queries.GetProducts
{
    public class GetProductsQueryHandler(IProductRepository productRepository, IMapper mapper)
        : IRequestHandler<GetProductsQuery, List<ProductDto>>
    {
        public async Task<List<ProductDto>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
        {
            var products = await productRepository.GetFilteredAsync(
                request.Name,
                request.MinPrice,
                request.MaxPrice,
                request.Availability,
                request.UserId,
                request.CreatedFrom,
                request.CreatedTo);

            return mapper.Map<List<ProductDto>>(products);
        }
    }
}
