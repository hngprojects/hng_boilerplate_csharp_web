using Hng.Application.Features.Categories.Dtos;
using MediatR;

namespace Hng.Application.Features.Categories.Queries
{
    public class GetCategoriesQuery : IRequest<IEnumerable<CategoryDto>>
    {
    }
}
