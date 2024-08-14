using AutoMapper;
using Hng.Application.Features.Blogs.Commands;
using Hng.Application.Features.Blogs.Dtos;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using Hng.Infrastructure.Services.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Hng.Application.Features.Blogs.Handlers;

public class CreateBlogCommandHandler(
    IMapper mapper,
    IRepository<Blog> blogRepository,
    IAuthenticationService authenticationService)
    : IRequestHandler<CreateBlogCommand, CreateBlogResponseDto>
{
    private readonly IMapper _mapper = mapper;
    private readonly IRepository<Blog> _blogRepository = blogRepository;
    private readonly IAuthenticationService _authenticationService = authenticationService;

    public async Task<CreateBlogResponseDto> Handle(CreateBlogCommand request, CancellationToken cancellationToken)
    {
        var userId = await _authenticationService.GetCurrentUserAsync();
        var blog = _mapper.Map<Blog>(request.BlogBody);
        blog.PublishedDate = DateTime.UtcNow;

        blog.AuthorId = userId;

        await _blogRepository.AddAsync(blog);
        await _blogRepository.SaveChanges();

        var blogDto = _mapper.Map<BlogDto>(blog);

        return new CreateBlogResponseDto
        {
            StatusCode = StatusCodes.Status201Created,
            Message = "Blog created successfully",
            Data = blogDto
        };
    }

}