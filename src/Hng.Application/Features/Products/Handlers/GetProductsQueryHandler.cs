using AutoMapper;
using Hng.Application.Features.Products.Dtos;
using Hng.Application.Features.Products.Queries;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using MediatR;

namespace Hng.Application.Features.Products.Handlers
{
	public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, IEnumerable<ProductDto>>
	{
		private readonly IRepository<Product> _productRepository;
		private readonly IMapper _mapper;

		public GetProductsQueryHandler(IMapper mapper, IRepository<Product> productRepository)
		{
			_mapper = mapper;
			_productRepository = productRepository;
		}

		public async Task<IEnumerable<ProductDto>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
		{
			var jobs = await _productRepository.GetAllAsync();
			return _mapper.Map<IEnumerable<ProductDto>>(jobs);
		}
	}
}
