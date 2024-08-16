using AutoMapper;
using Hng.Application.Features.Categories.Commands;
using Hng.Application.Features.Categories.Dtos;
using Hng.Application.Shared.Dtos;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using MediatR;

namespace Hng.Application.Features.Categories.Handlers.Commands
{
    public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, SuccessResponseDto<CategoryDto>>
    {
        private readonly IRepository<Category> _categoryRepository;
        private readonly IMapper _mapper;

        public CreateCategoryCommandHandler(IRepository<Category> categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<SuccessResponseDto<CategoryDto>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
            {
                return new SuccessResponseDto<CategoryDto>
                {
                    Data = null,
                    Message = "Category name is required.",
                    StatusCode = 400
                };
            }

            var category = new Category
            {
                Name = request.Name,
                Description = request.Description,
                Slug = request.Slug,
                CreatedAt = DateTime.UtcNow
            };

            await _categoryRepository.AddAsync(category);
            await _categoryRepository.SaveChanges();

            var categoryDto = _mapper.Map<CategoryDto>(category);
            return new SuccessResponseDto<CategoryDto>
            {
                Data = categoryDto,
                Message = "Category created successfully.",
                StatusCode = 201
            };
        }
    }
}