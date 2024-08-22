using Hng.Application.Features.Categories.Dtos;
using Hng.Application.Shared.Dtos;
using MediatR;

namespace Hng.Application.Features.Categories.Queries
{
    public record GetCategoryByIdQuery(Guid Id) : IRequest<SuccessResponseDto<CategoryDto>>
    {
    }
}