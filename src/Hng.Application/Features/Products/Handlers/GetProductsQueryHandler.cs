using AutoMapper;
using Hng.Application.Features.Products.Dtos;
using Hng.Application.Features.Products.Queries;
using Hng.Application.Shared.Dtos;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using MediatR;

namespace Hng.Application.Features.Products.Handlers
{
    public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, PagedListDto<ProductDto>>
    {
        private readonly IRepository<Product> _productRepository;
        private readonly IMapper _mapper;

        public GetProductsQueryHandler(IRepository<Product> productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<PagedListDto<ProductDto>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
        {
            var products = await _productRepository.GetAllAsync();

            var mappedProducts = _mapper.Map<IEnumerable<ProductDto>>(products);
            var productsResult = PagedListDto<ProductDto>.ToPagedList(mappedProducts, request.productsQueryParameters.PageNumber, request.productsQueryParameters.PageSize);

            return productsResult;
        }
    }
}
