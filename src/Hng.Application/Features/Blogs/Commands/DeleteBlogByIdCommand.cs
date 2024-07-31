using MediatR;

namespace Hng.Application.Features.Blogs.Commands;

public class DeleteBlogByIdCommand(Guid blogId) : IRequest<bool>
{
    public Guid BlogId { get; } = blogId;
}