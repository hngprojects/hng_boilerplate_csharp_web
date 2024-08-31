using Hng.Application.Features.Blogs.Commands;
using Hng.Application.Features.Blogs.Dtos;
using HotChocolate.Authorization;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Hng.Graphql
{
    public partial class Mutations
    {
        [Authorize]
        public async Task<CreateBlogResponseDto> CreateBlog(CreateBlogDto body, [FromServices] IMediator mediator)
        {
            var command = new CreateBlogCommand(body);
            return await mediator.Send(command);
        }

        [Authorize]
        public async Task<bool> DeleteBlogById(Guid id, [FromServices] IMediator mediator)
        {
            var command = (new DeleteBlogByIdCommand(id));
            return await mediator.Send(command);
        }

        [Authorize]
        public async Task<UpdateBlogResponseDto> UpdateBlog(Guid id, UpdateBlogDto updateBlogDto, [FromServices] IMediator mediator)
        {
            var command = new UpdateBlogCommand(updateBlogDto, id);
            return await mediator.Send(command);
        }
    }
}