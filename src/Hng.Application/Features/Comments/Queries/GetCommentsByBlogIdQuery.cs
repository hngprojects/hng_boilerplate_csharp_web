using Hng.Application.Features.Comments.Dtos;
using MediatR;

namespace Hng.Application.Features.Comments.Queries;

public class GetCommentsByBlogIdQuery(Guid blogId) : IRequest<IEnumerable<CommentDto>>
{
    public Guid BlogId { get; set; } = blogId;
}