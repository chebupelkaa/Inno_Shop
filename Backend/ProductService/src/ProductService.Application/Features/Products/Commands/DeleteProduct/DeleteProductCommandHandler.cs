using MediatR;
using Microsoft.AspNetCore.Http;
using ProductService.Application.Common;
using ProductService.Application.DTOs;
using ProductService.Application.Exceptions;
using ProductService.Domain.Interfaces;

namespace ProductService.Application.Features.Products.Commands.DeleteProduct
{
    public class DeleteProductCommandHandler(IProductRepository productRepository, IHttpContextAccessor httpContextAccessor)
        : IRequestHandler<DeleteProductCommand, Unit>
    {
        public async Task<Unit> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var product = await productRepository.GetByIdAsync(request.Id);
            if (product == null)
            {
                throw new NotFoundException(typeof(ProductDto), request.Id);
            }

            if (product.UserId != CurrentUser.GetUserId(httpContextAccessor))
            {
                throw new ForbiddenException("You can only delete your own products");
            }

            await productRepository.DeleteAsync(request.Id);
            await productRepository.SaveAsync();

            return Unit.Value;
        }
    }
}
