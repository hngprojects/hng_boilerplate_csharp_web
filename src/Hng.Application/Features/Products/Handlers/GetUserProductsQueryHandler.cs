using AutoMapper;
using Hng.Application.Features.Products.Dtos;
using Hng.Application.Features.Products.Queries;
using Hng.Application.Shared.Dtos;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using Hng.Infrastructure.Services.Interfaces;
using MediatR;

namespace Hng.Application.Features.Products.Handlers
{
    public class GetUserProductsQueryHandler : IRequestHandler<GetUserProductsQuery, PagedListDto<ProductDto>>
    {
        private readonly IRepository<Product> _productRepository;
        private readonly IMapper _mapper;
        private readonly IAuthenticationService _authenticationService;

        public GetUserProductsQueryHandler(IRepository<Product> productRepository, IMapper mapper, IAuthenticationService authenticationService)
        {
            _productRepository = productRepository;
            _mapper = mapper;
            _authenticationService = authenticationService;
        }

        public async Task<PagedListDto<ProductDto>> Handle(GetUserProductsQuery request, CancellationToken cancellationToken)
        {
            var userId = await _authenticationService.GetCurrentUserAsync();
            if (userId == Guid.Empty)
            {
                throw new ApplicationException("User ID is not available in the claims.");
            }

            var products = await _productRepository.GetAllBySpec(
            o => o.UserId == userId);

            var mappedProducts = _mapper.Map<IEnumerable<ProductDto>>(products);
            var productsResult = PagedListDto<ProductDto>.ToPagedList(mappedProducts, request.productsQueryParameters.PageNumber, request.productsQueryParameters.PageSize);

            return productsResult;
        }
    }
}
