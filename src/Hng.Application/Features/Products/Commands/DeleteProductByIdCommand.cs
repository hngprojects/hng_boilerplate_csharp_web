using MediatR;

namespace Hng.Application.Features.Products.Commands
{
    public class DeleteProductByIdCommand : IRequest
    {
        public DeleteProductByIdCommand(Guid id)
        {
            ProductId = id;
        }

        public Guid ProductId { get; }
    }
}
