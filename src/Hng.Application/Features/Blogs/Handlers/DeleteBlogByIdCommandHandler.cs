using Hng.Application.Features.Blogs.Commands;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using MediatR;

namespace Hng.Application.Features.Blogs.Handlers;

public class DeleteBlogByIdCommandHandler : IRequestHandler<DeleteBlogByIdCommand, bool>
{
    private readonly IRepository<Blog> _blogRepository;

    public DeleteBlogByIdCommandHandler(IRepository<Blog> blogRepository)
    {
        _blogRepository = blogRepository;
    }
    public async Task<bool> Handle(DeleteBlogByIdCommand request, CancellationToken cancellationToken)
    {
        var blog = await _blogRepository.GetBySpec(b => b.Id == request.BlogId);
        if (blog == null)
        {
            return false;
        }
        await _blogRepository.DeleteAsync(blog);
        await _blogRepository.SaveChanges();
        return true;
    }
}