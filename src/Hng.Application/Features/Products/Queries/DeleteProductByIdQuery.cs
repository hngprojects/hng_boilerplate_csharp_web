using Hng.Application.Features.Products.Enums;
using MediatR;

namespace Hng.Application.Features.Products.Queries
{
    public class DeleteProductByIdQuery : IRequest<ProductQueryStatusEnum>
    {
        public DeleteProductByIdQuery(Guid id)
        {
            ProductId = id;
        }

        public Guid ProductId { get; }
    }
}
