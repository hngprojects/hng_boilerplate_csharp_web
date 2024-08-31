using Hng.Application.Features.Blogs.Dtos;
using Hng.Application.Features.Blogs.Queries;
using HotChocolate.Authorization;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Hng.Graphql
{
    public partial class Queries
    {
        [Authorize]
        public async Task<GetBlogResponseDto> GetBlogById(Guid id, [FromServices] IMediator mediator)
        {
            var query = new GetBlogByIdQuery(id);
            return await mediator.Send(query);
        }

        [Authorize]
        public async Task<IEnumerable<BlogDto>> GetBlogs([FromServices] IMediator mediator)
        {
            var blogs = new GetBlogsQuery();
            return await mediator.Send(blogs);
        }
    }
}