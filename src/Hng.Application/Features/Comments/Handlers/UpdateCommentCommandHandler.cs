using AutoMapper;
using Hng.Application.Features.Comments.Commands;
using Hng.Application.Features.Comments.Dtos;
using Hng.Application.Shared.Dtos;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using Hng.Infrastructure.Services.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Hng.Application.Features.Comments.Handlers;

public class UpdateCommentCommandHandler(
    IMapper mapper,
    IRepository<Comment> commentRepository,
    IAuthenticationService authenticationService)
    : IRequestHandler<UpdateCommentCommand, SuccessResponseDto<CommentDto>>
{
    private readonly IMapper _mapper = mapper;
    private readonly IRepository<Comment> _commentRepository = commentRepository;
    private readonly IAuthenticationService _authenticationService = authenticationService;

    public async Task<SuccessResponseDto<CommentDto>> Handle(UpdateCommentCommand request, CancellationToken cancellationToken)
    {
        var comment = await _commentRepository.GetBySpec(c => c.Id == request.commentId && c.BlogId == request.BlogId);
        if (comment == null)
        {
            throw new KeyNotFoundException("Comment not found.");
        }

        var userId = await _authenticationService.GetCurrentUserAsync();
        if (comment.AuthorId != userId)
        {
            throw new UnauthorizedAccessException("You are not authorized to update this comment.");
        }

        if (string.IsNullOrWhiteSpace(request.CommentBody.Content))
        {
            throw new ArgumentException("Content cannot be empty.");
        }

        // Update only the content field
        comment.Content = request.CommentBody.Content;
        comment.UpdatedAt = DateTime.UtcNow;

        await _commentRepository.UpdateAsync(comment);
        await _commentRepository.SaveChanges();

        return new SuccessResponseDto<CommentDto>
        {
            Data = _mapper.Map<CommentDto>(comment),
            Message = "Comment updated successfully",
            StatusCode = 200
        };
    }
}
