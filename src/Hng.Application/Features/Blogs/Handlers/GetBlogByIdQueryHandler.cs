using AutoMapper;
using Hng.Application.Features.Blogs.Dtos;
using Hng.Application.Features.Blogs.Queries;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using MediatR;

namespace Hng.Application.Features.Blogs.Handlers;

public class GetBlogByIdQueryHandler : IRequestHandler<GetBlogByIdQuery, BlogDto>
{
    private readonly IMapper _mapper;
    private readonly IRepository<Blog> _blogRepository;

    public GetBlogByIdQueryHandler(IMapper mapper, IRepository<Blog> blogRepository)
    {
        _mapper = mapper;
        _blogRepository = blogRepository;
    }

    public async Task<BlogDto> Handle(GetBlogByIdQuery request, CancellationToken cancellationToken)
    {
        var blog = await _blogRepository.GetBySpec(b => b.Id == request.BlogId);
        return blog == null ? null : _mapper.Map<BlogDto>(blog);
    }
}