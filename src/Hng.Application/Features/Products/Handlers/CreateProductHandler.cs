using AutoMapper;
using Hng.Application.Features.Products.Commands;
using Hng.Application.Features.Products.Dtos;
using Hng.Domain.Entities;
using Hng.Infrastructure.Services.Interfaces;
using Hng.Infrastructure.Repository.Interface;
using MediatR;

namespace Hng.Application.Features.Products.Handlers
{
    public class CreateProductHandler : IRequestHandler<CreateProductCommand, CreateProductResponseDto>
    {
        private readonly IRepository<Product> _productRepository;
        private readonly IRepository<Organization> _organizationRepository;
        private readonly IAuthenticationService _authenticationService;
        private readonly IMapper _mapper;

        public CreateProductHandler(IRepository<Product> productReposiotry, IRepository<Organization> organizationRepository, IAuthenticationService authenticationService, IMapper mapper)
        {
            _productRepository = productReposiotry;
            _organizationRepository = organizationRepository;
            _authenticationService = authenticationService;
            _mapper = mapper;
        }
        public async Task<CreateProductResponseDto> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var userId = await _authenticationService.GetCurrentUserAsync();
            var organization = await _organizationRepository.GetBySpec(u => u.Id == request.OrgId);
            if (organization != null)
            {
                var product = _mapper.Map<Product>(request.ProductDto);
                product.OrganizationId = request.OrgId;
                product.UserId = userId;
                await _productRepository.AddAsync(product);
                await _productRepository.SaveChanges();
                return _mapper.Map<CreateProductResponseDto>(product);
            }
            return null;
        }
    }
}
