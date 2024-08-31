using Hng.Application.Features.Comments.Dtos;
using Hng.Application.Features.Comments.Queries;
using HotChocolate.Authorization;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Hng.Graphql
{
    public partial class Queries
    {
        [Authorize]
        public async Task<IEnumerable<CommentDto>> GetCommentsByBlogId(Guid blogId, [FromServices] IMediator mediator)
        {
            var query = new GetCommentsByBlogIdQuery(blogId);
            return await mediator.Send(query);
        }
    }
}