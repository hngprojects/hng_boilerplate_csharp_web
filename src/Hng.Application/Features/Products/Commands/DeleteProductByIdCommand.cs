using Hng.Application.Features.Products.Dtos;
using MediatR;

namespace Hng.Application.Features.Products.Commands
{
    public class DeleteProductByIdCommand : IRequest<ProductDto>
    {
        public DeleteProductByIdCommand(Guid id)
        {
            ProductId = id;
        }

        public Guid ProductId { get; }
    }
}
