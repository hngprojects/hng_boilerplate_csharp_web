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
        private readonly IRepository<Category> _categoryRepository;
        private readonly IMapper _mapper;

        public GetAllProductsHandler(IRepository<Product> productRepository, IRepository<Category> categoryRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProductResponseDto>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            var validCategories = await _categoryRepository.GetAllAsync();
            var categoriesNames = validCategories.Select(category => category.Name).ToHashSet();

            if (!string.IsNullOrEmpty(request.Category) && !categoriesNames.Contains(request.Category)){
                throw new ArgumentException($"Invalid category '{request.Category}' provided.");
            }

            var products = await _productRepository.GetAllBySpec(product => product.OrganizationId == request.OrgId && (string.IsNullOrEmpty(request.Category) || product.Category == request.Category));
            var productDtos = _mapper.Map<IEnumerable<ProductResponseDto>>(products);

            foreach (var product in productDtos)
            {
                product.Status = product.Quantity > 0 ? "in stock" : "out of stock";
            }

            return productDtos;
        }
    }

}
