using AutoMapper;
using Hng.Application.Features.Products.Enums;
using Hng.Application.Features.Products.Queries;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using MediatR;

namespace Hng.Application.Features.Products.Handlers
{
    public class DeleteProductByIdQueryHandler : IRequestHandler<DeleteProductByIdQuery, ProductQueryStatusEnum>
    {
        private readonly IRepository<Category> _categoryRepository;
        private readonly IMapper _mapper;

        public DeleteProductByIdQueryHandler(IRepository<Category> categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<ProductQueryStatusEnum> Handle(DeleteProductByIdQuery request, CancellationToken cancellationToken)
        {
            var category = await _categoryRepository.GetAsync(request.ProductId);

            if (category != null)
            {
                await _categoryRepository.DeleteAsync(category);
                await _categoryRepository.SaveChanges();
                return ProductQueryStatusEnum.Success;
            }

            return ProductQueryStatusEnum.NotFound;
        }
    }
}
