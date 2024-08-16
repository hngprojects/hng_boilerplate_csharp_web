using Hng.Application.Features.Categories.Commands;
using Hng.Application.Shared.Dtos;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using MediatR;

namespace Hng.Application.Features.Categories.Handlers.Commands
{
    public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand, SuccessResponseDto<bool>>
    {
        private readonly IRepository<Category> _categoryRepository;

        public DeleteCategoryCommandHandler(IRepository<Category> categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<SuccessResponseDto<bool>> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await _categoryRepository.GetAsync(request.Id);
            if (category == null)
                return new SuccessResponseDto<bool>
                {
                    Data = false,
                    Message = "Category not found.",
                    StatusCode = 404
                };

            await _categoryRepository.DeleteAsync(category);
            await _categoryRepository.SaveChanges();

            return new SuccessResponseDto<bool>
            {
                Data = true,
                Message = "Category deleted successfully.",
                StatusCode = 200
            };
        }
    }
}
