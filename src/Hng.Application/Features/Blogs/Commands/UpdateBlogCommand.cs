using Hng.Application.Features.Blogs.Dtos;
using MediatR;

namespace Hng.Application.Features.Blogs.Commands;

public class UpdateBlogCommand(UpdateBlogDto blog, Guid id) : IRequest<UpdateBlogResponseDto>
{
    public Guid BlogId { get; } = id;
    public UpdateBlogDto Blog { get; } = blog;
}