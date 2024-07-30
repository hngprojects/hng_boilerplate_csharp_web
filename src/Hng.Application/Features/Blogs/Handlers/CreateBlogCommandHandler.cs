using AutoMapper;
using Hng.Application.Features.Blogs.Commands;
using Hng.Application.Features.Blogs.Dtos;
using Hng.Domain.Entities;
using Hng.Domain.Enums;
using Hng.Infrastructure.Repository.Interface;
using MediatR;

namespace Hng.Application.Features.Blogs.Handlers;

public class CreateBlogCommandHandler : IRequestHandler<CreateBlogCommand, BlogDto>
{
    
    private readonly IMapper _mapper;
    private readonly IRepository<Blog> _blogRepository;


    public CreateBlogCommandHandler(IMapper mapper, IRepository<Blog> blogRepository)
    {
        _mapper = mapper;
        _blogRepository = blogRepository;
    }

    public async Task<BlogDto> Handle(CreateBlogCommand request, CancellationToken cancellationToken)
    {

        var blog = _mapper.Map<Blog>(request.BlogBody);
        blog.PublishedDate = DateTime.UtcNow;

        await _blogRepository.AddAsync(blog);
        await _blogRepository.SaveChanges();

        return _mapper.Map<BlogDto>(blog);
    }
}