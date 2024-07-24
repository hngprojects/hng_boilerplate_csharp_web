using AutoMapper;
using Hng.Application.Features.Products.Commands;
using Hng.Application.Features.Products.Dtos;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hng.Application.Features.Products.Handlers
{
    public class CreateProductHandler: IRequestHandler<CreateProductCommand, ProductDto>
    {
        private IRepository<Product> _productRepo;
        private IMapper _mapper;

        public CreateProductHandler(IRepository<Product> productreposiotry, IMapper mapper)
        {
            _productRepo = productreposiotry;
            _mapper = mapper;
        }

        public async Task<ProductDto> Handle(CreateProductCommand    request, CancellationToken cancellationToken)
        {
            var product = _mapper.Map<Product>(request.productBody);
            await _productRepo.AddAsync(product);
            return _mapper.Map<ProductDto>(product);
        }
    }
}
