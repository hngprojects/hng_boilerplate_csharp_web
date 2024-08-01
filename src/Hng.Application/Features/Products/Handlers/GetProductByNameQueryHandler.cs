using AutoMapper;
using Hng.Application.Features.Products.Dtos;
using Hng.Application.Features.Products.Queries;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using MediatR;

namespace Hng.Application.Features.Products.Handlers
{
    public class GetProductByNameQueryHandler : IRequestHandler<GetProductByNameQuery, ProductDto>
    {
        private readonly IRepository<Product> _productRepository;
        private readonly IMapper _mapper;

        public GetProductByNameQueryHandler(IRepository<Product> productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<ProductDto> Handle(GetProductByNameQuery request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetBySpec(n => n.Name == request.Name);
            return product != null ? _mapper.Map<ProductDto>(product) : null;
        }
    }
}
