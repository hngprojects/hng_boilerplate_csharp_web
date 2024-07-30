using AutoMapper;
using Hng.Application.Features.Blogs.Dtos;
using Hng.Application.Features.Blogs.Queries;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using MediatR;

namespace Hng.Application.Features.Blogs.Handlers;

public class GetBlogsQueryHandler : IRequestHandler<GetBlogsQuery, IEnumerable<BlogDto>>
{
    private readonly IRepository<Blog> _blogRepository;
    private readonly IMapper _mapper;

    public GetBlogsQueryHandler(IRepository<Blog> blogRepository, IMapper mapper)
    {
        _blogRepository = blogRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<BlogDto>> Handle(GetBlogsQuery request, CancellationToken cancellationToken)
    {
        var blogs = await _blogRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<BlogDto>>(blogs);
    }
}