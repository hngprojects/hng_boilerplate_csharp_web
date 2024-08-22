using AutoMapper;
using Hng.Application.Features.Categories.Dtos;
using Hng.Application.Features.Categories.Queries;
using Hng.Application.Shared.Dtos;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using MediatR;

namespace Hng.Application.Features.Categories.Handlers.Queries
{
    public class GetCategoryByIdQueryHandler : IRequestHandler<GetCategoryByIdQuery, SuccessResponseDto<CategoryDto>>
    {
        private readonly IRepository<Category> _categoryRepository;
        private readonly IMapper _mapper;

        public GetCategoryByIdQueryHandler(IRepository<Category> categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<SuccessResponseDto<CategoryDto>> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
        {
            var category = await _categoryRepository.GetAsync(request.Id);
            if (category == null)
                return new SuccessResponseDto<CategoryDto>
                {
                    Data = null,
                    Message = "Category not found.",
                    StatusCode = 404
                };

            var categoryDto = _mapper.Map<CategoryDto>(category);
            return new SuccessResponseDto<CategoryDto>
            {
                Data = categoryDto,
                Message = "Category retrieved successfully.",
                StatusCode = 200
            };
        }
    }
}