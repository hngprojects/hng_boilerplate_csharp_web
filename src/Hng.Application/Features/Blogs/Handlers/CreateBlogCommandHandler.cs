using AutoMapper;
using Hng.Application.Features.Blogs.Commands;
using Hng.Application.Features.Blogs.Dtos;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using Hng.Infrastructure.Services.Interfaces;
using MediatR;

namespace Hng.Application.Features.Blogs.Handlers;

public class CreateBlogCommandHandler : IRequestHandler<CreateBlogCommand, BlogDto>
{
    private readonly IMapper _mapper;
    private readonly IRepository<Blog> _blogRepository;
    private readonly IAuthenticationService _authenticationService;

    public CreateBlogCommandHandler(IMapper mapper, IRepository<Blog> blogRepository, IAuthenticationService authenticationService)
    {
        _mapper = mapper;
        _blogRepository = blogRepository;
        _authenticationService = authenticationService;

    }

    public async Task<BlogDto> Handle(CreateBlogCommand request, CancellationToken cancellationToken)
    {
        
        var user = await _authenticationService.GetCurrentUserAsync();
        
        var blog = _mapper.Map<Blog>(request.BlogBody);
        blog.PublishedDate = DateTime.UtcNow;
        blog.Author = user;

        await _blogRepository.AddAsync(blog);
        await _blogRepository.SaveChanges();

        return _mapper.Map<BlogDto>(blog);
        
        
    }
    

}