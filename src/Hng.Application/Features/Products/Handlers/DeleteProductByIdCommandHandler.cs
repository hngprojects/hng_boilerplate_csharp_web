using Hng.Application.Features.Products.Commands;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using MediatR;

namespace Hng.Application.Features.Products.Handlers
{
    public class DeleteProductByIdCommandHandler : IRequestHandler<DeleteProductByIdCommand>
    {
        private readonly IRepository<Product> _productRepository;

        public DeleteProductByIdCommandHandler(IRepository<Product> productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task Handle(DeleteProductByIdCommand request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetAsync(request.ProductId);

            if (product != null)
            {
                await _productRepository.DeleteAsync(product);
                await _productRepository.SaveChanges();
            }
        }
    }
}
