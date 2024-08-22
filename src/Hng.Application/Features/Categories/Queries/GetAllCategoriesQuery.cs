using Hng.Application.Features.Categories.Dtos;
using Hng.Application.Shared.Dtos;
using MediatR;

namespace Hng.Application.Features.Categories.Queries
{
    public record GetAllCategoriesQuery(GetAllCategoriesQueryParams QueryParams) : IRequest<PaginatedResponseDto<List<CategoryDto>>>
    {
    }
}