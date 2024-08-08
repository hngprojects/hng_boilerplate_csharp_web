using AutoMapper;
using Hng.Application.Features.Blogs.Commands;
using Hng.Application.Features.Blogs.Dtos;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using Hng.Infrastructure.Services.Interfaces;
using MediatR;

namespace Hng.Application.Features.Blogs.Handlers;

public class UpdateBlogCommandHandler(
    IMapper mapper,
    IRepository<Blog> blogRepository,
    IAuthenticationService authenticationService)
    : IRequestHandler<UpdateBlogCommand, BlogDto>
{
    private readonly IMapper _mapper = mapper;
    private readonly IRepository<Blog> _blogRepository = blogRepository;
    private readonly IAuthenticationService _authenticationService = authenticationService;

    public async  Task<BlogDto> Handle(UpdateBlogCommand request, CancellationToken cancellationToken)
    {
        var blog = await _blogRepository.GetBySpec(b => b.Id == request.BlogId);
        var loggedInUserId = await _authenticationService.GetCurrentUserAsync();

        if (blog.AuthorId != loggedInUserId)
        {
            throw new UnauthorizedAccessException("You do not have permission to update this blog.");
        }

        blog.Title = request.Blog.Title;
        blog.Category = request.Blog.Category;
        blog.Content = request.Blog.Content;
        blog.UpdatedDate = DateTime.UtcNow;
        blog.ImageUrl = request.Blog.ImageUrl;

        await _blogRepository.UpdateAsync(blog);

        await _blogRepository.SaveChanges();
        return _mapper.Map<BlogDto>(blog);
    }
}