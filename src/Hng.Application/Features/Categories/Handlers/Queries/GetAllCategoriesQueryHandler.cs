using AutoMapper;
using Hng.Application.Features.Categories.Dtos;
using Hng.Application.Features.Categories.Queries;
using Hng.Application.Shared.Dtos;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using MediatR;

namespace Hng.Application.Features.Categories.Handlers.Queries
{
    public class GetAllCategoriesQueryHandler : IRequestHandler<GetAllCategoriesQuery, PaginatedResponseDto<List<CategoryDto>>>
    {
        private readonly IRepository<Category> _categoryRepository;
        private readonly IMapper _mapper;

        public GetAllCategoriesQueryHandler(IRepository<Category> categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<PaginatedResponseDto<List<CategoryDto>>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
        {
            var categories = await _categoryRepository.GetAllAsync();
            var categoryDtos = _mapper.Map<List<CategoryDto>>(categories);

            var pagedCategories = PagedListDto<CategoryDto>.ToPagedList(
                categoryDtos,
                request.QueryParams.Offset,
                request.QueryParams.Limit
            );

            return new PaginatedResponseDto<List<CategoryDto>>
            {
                Data = pagedCategories,
                Metadata = pagedCategories.MetaData,
                Message = "Categories retrieved successfully."
            };
        }
    }
}