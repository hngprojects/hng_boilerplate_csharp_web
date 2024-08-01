using AutoMapper;
using Hng.Application.Features.Products.Dtos;
using Hng.Application.Features.Products.Queries;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using MediatR;


namespace Hng.Application.Features.Products.Handlers
{
    public class GetProductsByNameQueryHandler : IRequestHandler<GetProductByNameQuery, List<ProductDto>>
    {
        private readonly IRepository<Product> _productRepository;
        private readonly IMapper _mapper;

        public GetProductsByNameQueryHandler(IRepository<Product> productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }
        public async Task<List<ProductDto>> Handle(GetProductByNameQuery request, CancellationToken cancellationToken)
        {
            var products = await _productRepository.GetAllAsync();
            if (!string.IsNullOrEmpty(request.Name))
            { products = products.Where(p => p.Name.Contains(request.Name, StringComparison.OrdinalIgnoreCase)).ToList(); }
            return _mapper.Map<List<ProductDto>>(products);
        }

    }
}
