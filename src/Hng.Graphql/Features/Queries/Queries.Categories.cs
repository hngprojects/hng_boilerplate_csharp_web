using Hng.Application.Features.Categories.Dtos;
using Hng.Application.Features.Categories.Queries;
using Hng.Application.Shared.Dtos;
using HotChocolate.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;

namespace Hng.Graphql
{
    public partial class Queries
    {
        [Authorize]
        public async Task<SuccessResponseDto<CategoryDto>> GetCategory(Guid id, [FromServices] IMediator mediator)
        {
            var query = new GetCategoryByIdQuery(id);
            return await mediator.Send(query);
        }

        [Authorize]
        public async Task<PaginatedResponseDto<List<CategoryDto>>> GetAllCategories(GetAllCategoriesQueryParams queryParams, [FromServices] IMediator mediator)
        {
            var query = new GetAllCategoriesQuery(queryParams);
            return await mediator.Send(query);
        }
    }
}
