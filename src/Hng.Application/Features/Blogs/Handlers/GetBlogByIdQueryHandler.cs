using AutoMapper;
using Hng.Application.Features.Blogs.Dtos;
using Hng.Application.Features.Blogs.Queries;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using MediatR;

namespace Hng.Application.Features.Blogs.Handlers;

public class GetBlogByIdQueryHandler(IRepository<Blog> blogRepository, IMapper mapper) : IRequestHandler<GetBlogByIdQuery, BlogDto>
{
    public async Task<BlogDto> Handle(GetBlogByIdQuery request, CancellationToken cancellationToken)
    {
        var blog = await blogRepository.GetBySpec(b => b.Id == request.BlogId);
        return blog == null ? null : mapper.Map<BlogDto>(blog);
    }
}