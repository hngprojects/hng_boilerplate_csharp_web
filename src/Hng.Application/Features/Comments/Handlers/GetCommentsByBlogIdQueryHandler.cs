using AutoMapper;
using Hng.Application.Features.Comments.Dtos;
using Hng.Application.Features.Comments.Queries;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using MediatR;

namespace Hng.Application.Features.Comments.Handlers;

public class GetCommentsByBlogIdQueryHandler(IMapper mapper, IRepository<Comment> commentRepository) : IRequestHandler<GetCommentsByBlogIdQuery, IEnumerable<CommentDto>>
{
    private readonly IRepository<Comment> _commentRepository = commentRepository;
    private readonly IMapper _mapper = mapper;
    
    public async Task<IEnumerable<CommentDto>> Handle(GetCommentsByBlogIdQuery request, CancellationToken cancellationToken)
    {
        var comments = await _commentRepository.GetAllBySpec(c => c.BlogId == request.BlogId);
        return _mapper.Map<IEnumerable<CommentDto>>(comments);
    }
}