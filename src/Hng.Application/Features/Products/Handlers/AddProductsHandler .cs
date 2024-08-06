using AutoMapper;
using Hng.Application.Features.Products.Commands;
using Hng.Application.Features.Products.Dtos;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using MediatR;

namespace Hng.Application.Features.Products.Handlers
{
	public class AddProductsHandler : IRequestHandler<AddProductsCommand, ProductsDto>
    {
        private readonly IRepository<Product> _repository;
        private readonly IMapper _mapper;

        public AddProductsHandler(IRepository<Product> productReposiotry, IMapper mapper)
        {
            _repository = productReposiotry;
            _mapper = mapper;
        }
        public async Task<ProductsDto> Handle(AddProductsCommand request, CancellationToken cancellationToken)
        {
            var productResponse = new ProductsDto();
            
            foreach(var item in request.productBody)
            {
				var product = _mapper.Map<Product>(item);
				product.Id = Guid.NewGuid();
				product.UserId = Guid.Parse(request.UserId);
				product.CreatedAt = DateTime.UtcNow;
				product.UpdatedAt = DateTime.UtcNow;
				await _repository.AddAsync(product);

                productResponse.Products.Add(_mapper.Map<ProductDto>(product));
			}
            await _repository.SaveChanges();

            return productResponse;
        }
    }
}
