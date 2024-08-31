using Hng.Application.Features.Comments.Commands;
using Hng.Application.Features.Comments.Dtos;
using HotChocolate.Authorization;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Hng.Graphql
{
    public partial class Mutations
    {
        [Authorize]
        public async Task<CommentDto> CreateComment(Guid blogId, CreateCommentDto body, [FromServices] IMediator mediator)
        {
            var command = new CreateCommentCommand(body, blogId);
            return await mediator.Send(command);
        }
    }
}