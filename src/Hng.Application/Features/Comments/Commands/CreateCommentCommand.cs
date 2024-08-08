using Hng.Application.Features.Comments.Dtos;
using MediatR;

namespace Hng.Application.Features.Comments.Commands;

public class CreateCommentCommand(CreateCommentDto createCommentDto, Guid blogId) : IRequest<CommentDto>
{
    public Guid BlogId { get; } = blogId;
    public CreateCommentDto CommentBody { get; } = createCommentDto;
}