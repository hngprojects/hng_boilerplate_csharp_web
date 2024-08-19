using AutoMapper;
using Hng.Application.Features.Blogs.Dtos;
using Hng.Application.Features.Blogs.Queries;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using MediatR;

namespace Hng.Application.Features.Blogs.Handlers;

public class GetBlogByIdQueryHandler(IMapper mapper, IRepository<Blog> blogRepository)
    : IRequestHandler<GetBlogByIdQuery, GetBlogResponseDto>
{
    private readonly IMapper _mapper = mapper;
    private readonly IRepository<Blog> _blogRepository = blogRepository;

    public async Task<GetBlogResponseDto> Handle(GetBlogByIdQuery request, CancellationToken cancellationToken)
    {
        var blog = await _blogRepository.GetBySpec(b => b.Id == request.BlogId);
        return blog == null ? null : _mapper.Map<BlogDto>(blog);
    }
}