using Hng.Application.Features.Categories.Commands;
using Hng.Application.Features.Categories.Dtos;
using Hng.Application.Shared.Dtos;
using HotChocolate.Authorization;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Hng.Graphql
{
    public partial class Mutations
    {
        [Authorize]
        public async Task<SuccessResponseDto<CategoryDto>> CreateCategory(CreateCategoryDto createCategoryDto, [FromServices] IMediator mediator)
        {
            var command = new CreateCategoryCommand(createCategoryDto.Name,createCategoryDto.Description, createCategoryDto.Slug);
            return await mediator.Send(command);
        }

        [Authorize]
        public async Task<SuccessResponseDto<bool>> DeleteCategory(Guid id, [FromServices] IMediator mediator)
        {
            var command = new DeleteCategoryCommand(id);
            return await mediator.Send(command);
        }

    }
}