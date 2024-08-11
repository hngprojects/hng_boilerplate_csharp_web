using AutoMapper;
using Hng.Application.Features.Products.Dtos;
using Hng.Application.Features.Products.Queries;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using MediatR;

namespace Hng.Application.Features.Products.Handlers
{
    public class GetAllProductsHandler : IRequestHandler<GetAllProductsQuery, IEnumerable<ProductResponseDto>>
    {
        private readonly IRepository<Product> _productRepository;
        private readonly IMapper _mapper;

        public GetAllProductsHandler(IRepository<Product> productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProductResponseDto>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            var products = await _productRepository.GetAllBySpec(u => u.OrganizationId == request.OrgId);
            var productDtos = _mapper.Map<IEnumerable<ProductResponseDto>>(products);

            foreach (var product in productDtos)
            {
                product.Status = product.Quantity > 0 ? "in stock" : "out of stock";
            }

            return productDtos;
        }
    }

}
