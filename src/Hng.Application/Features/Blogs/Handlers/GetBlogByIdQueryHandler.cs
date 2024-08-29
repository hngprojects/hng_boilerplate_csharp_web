using AutoMapper;
using Hng.Application.Features.Blogs.Dtos;
using Hng.Application.Features.Blogs.Queries;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Hng.Application.Features.Blogs.Handlers;

public class GetBlogByIdQueryHandler(IMapper mapper, IRepository<Blog> blogRepository)
    : IRequestHandler<GetBlogByIdQuery, GetBlogResponseDto>
{
    private readonly IMapper _mapper = mapper;
    private readonly IRepository<Blog> _blogRepository = blogRepository;

    public async Task<GetBlogResponseDto> Handle(GetBlogByIdQuery request, CancellationToken cancellationToken)
    {
        var blog = await _blogRepository.GetBySpec(b => b.Id == request.BlogId);

        if (blog is null)
        {
            return new GetBlogResponseDto
            {
                StatusCode = StatusCodes.Status404NotFound,
                Message = "Blog Not Found",
            };
        }

        var blogDto = _mapper.Map<BlogDto>(blog);

        return new GetBlogResponseDto
        {
            StatusCode = StatusCodes.Status200OK,
            Message = "Blog Successfully Retrieved",
            Data = blogDto
        };
    }
}