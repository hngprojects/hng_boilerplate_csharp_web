using Hng.Application.Features.Comments.Dtos;
using Hng.Application.Shared.Dtos;
using MediatR;

namespace Hng.Application.Features.Comments.Commands
{
    public class UpdateCommentCommand(Guid blogId, Guid commentId, UpdateCommentDto body, Guid userId) : IRequest<SuccessResponseDto<CommentDto>>
    {
        public Guid BlogId { get; } = blogId;
        public Guid commentId { get; } = commentId;
        public UpdateCommentDto CommentBody { get; } = body;
        public Guid userId { get; } = userId;
    }
}
