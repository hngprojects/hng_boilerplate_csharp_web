using Hng.Application.Features.Products.Dtos;
using MediatR;

namespace Hng.Application.Features.Products.Commands
{
    public class UpdateProductCommand : IRequest<ProductDto>
    {
        public UpdateProductCommand(Guid id, UpdateProductDto updateProductDto)
        {
            Id = id;
            UpdateProductDto = updateProductDto;
        }

        public Guid Id { get; }
        public UpdateProductDto UpdateProductDto { get; }
    }
}
