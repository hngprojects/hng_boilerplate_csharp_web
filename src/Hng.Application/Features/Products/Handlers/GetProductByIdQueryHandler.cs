using System;
using Hng.Application.Features.Products.Queries;
using Hng.Application.Features.Products.Dtos;
using Hng.Infrastructure.Repository.Interface;
using MediatR;
using AutoMapper;
using Hng.Domain.Entities;

namespace Hng.Application.Features.Products.Handlers
{
  public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ProductDto>
  {
    private readonly IRepository<Product> _productRepository;
    private readonly IMapper _mapper;

    public GetProductByIdQueryHandler(IRepository<Product> productRepository, IMapper mapper)
    {
      _productRepository = productRepository;
      _mapper = mapper;
    }

    public async Task<ProductDto> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
      var product = await _productRepository.GetByIdAsync(request.Id);
      if (product == null)
      {
        throw new KeyNotFoundException($"Product with ID {request.Id} not found.");
      }

      return _mapper.Map<ProductDto>(product);
    }
  }
}
