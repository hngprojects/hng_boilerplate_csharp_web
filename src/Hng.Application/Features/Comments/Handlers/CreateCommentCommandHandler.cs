using AutoMapper;
using Hng.Application.Features.Comments.Commands;
using Hng.Application.Features.Comments.Dtos;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using Hng.Infrastructure.Services.Interfaces;
using MediatR;

namespace Hng.Application.Features.Comments.Handlers;

public class CreateCommentCommandHandler(
    IMapper mapper,
    IRepository<Comment> commentRepository,
    IRepository<Blog> blogRepository,
    IAuthenticationService authenticationService)
    : IRequestHandler<CreateCommentCommand, CommentDto>
{
    private readonly IMapper _mapper = mapper;
    private readonly IRepository<Comment> _commentRepository = commentRepository;
    private readonly IRepository<Blog> _blogRepository = blogRepository;
    private readonly IAuthenticationService _authenticationService = authenticationService;

    public async Task<CommentDto> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var userId = await _authenticationService.GetCurrentUserAsync();
        if (userId == Guid.Empty)
        {
            throw new ApplicationException("User ID is not available in the claims.");
        }

        var blog = await _blogRepository.GetBySpec(b => b.Id == request.BlogId);
        if (blog == null)
        {
            throw new ApplicationException("Blog not found");
        }

        var comment = _mapper.Map<Comment>(request.CommentBody);
        if (comment == null)
        {
            throw new ApplicationException("Failed to map comment.");
        }

        comment.AuthorId = userId;
        comment.BlogId = request.BlogId;
        comment.CreatedAt = DateTime.UtcNow;

        await _commentRepository.AddAsync(comment);
        await _commentRepository.SaveChanges();

        blog.Comments ??= new List<Comment>();

        blog.Comments.Add(comment);

        await _blogRepository.SaveChanges();

        return _mapper.Map<CommentDto>(comment);
    }
}