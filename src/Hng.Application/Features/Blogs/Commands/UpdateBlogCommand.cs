using Hng.Application.Features.Blogs.Dtos;
using MediatR;

namespace Hng.Application.Features.Blogs.Commands;

public class UpdateBlogCommand(BlogDto blog) : IRequest<BlogDto>
{
    public BlogDto BlogBody { get; } = blog;
}