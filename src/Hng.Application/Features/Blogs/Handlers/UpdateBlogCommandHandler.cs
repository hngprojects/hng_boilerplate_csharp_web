using AutoMapper;
using Hng.Application.Features.Blogs.Commands;
using Hng.Application.Features.Blogs.Dtos;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using Hng.Infrastructure.Services.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Hng.Application.Features.Blogs.Handlers;

public class UpdateBlogCommandHandler(
    IMapper mapper,
    IRepository<Blog> blogRepository,
    IAuthenticationService authenticationService)
    : IRequestHandler<UpdateBlogCommand, UpdateBlogResponseDto>
{
    private readonly IMapper _mapper = mapper;
    private readonly IRepository<Blog> _blogRepository = blogRepository;
    private readonly IAuthenticationService _authenticationService = authenticationService;

    public async Task<UpdateBlogResponseDto> Handle(UpdateBlogCommand request, CancellationToken cancellationToken)
    {
        var blog = await _blogRepository.GetBySpec(b => b.Id == request.BlogId);
        var loggedInUserId = await _authenticationService.GetCurrentUserAsync();

        if (blog.AuthorId != loggedInUserId)
        {
            return new UpdateBlogResponseDto
            {
                StatusCode = StatusCodes.Status403Forbidden,
                Message = "You do not have permission to update this blog.",
            };
        }

        blog.Title = request.Blog.Title;
        blog.Category = request.Blog.Category;
        blog.Content = request.Blog.Content;
        blog.UpdatedDate = DateTime.UtcNow;
        blog.ImageUrl = request.Blog.ImageUrl;

        await _blogRepository.UpdateAsync(blog);

        await _blogRepository.SaveChanges();
        var blogDto = _mapper.Map<BlogDto>(blog);

        return new UpdateBlogResponseDto
        {
            StatusCode = 200,
            Message = "Blog updated successfully",
            Data = blogDto
        };
    }
}