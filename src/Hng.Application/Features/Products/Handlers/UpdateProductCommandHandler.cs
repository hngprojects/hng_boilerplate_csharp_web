using AutoMapper;
using Hng.Application.Features.Products.Commands;
using Hng.Application.Features.Products.Dtos;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using MediatR;

namespace Hng.Application.Features.Products.Handlers
{
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, ProductDto>
    {
        private readonly IRepository<Product> _productRepository;
        private readonly IMapper _mapper;

        public UpdateProductCommandHandler(IRepository<Product> productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<ProductDto> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetAsync(request.Id);

            if (product != null)
            {
                _mapper.Map(request.UpdateProductDto, product);
                product.UpdatedAt = DateTime.UtcNow;

                await _productRepository.UpdateAsync(product);
                await _productRepository.SaveChanges();

                return _mapper.Map<ProductDto>(product);
            }

             return null;
           
        }
    }
}
