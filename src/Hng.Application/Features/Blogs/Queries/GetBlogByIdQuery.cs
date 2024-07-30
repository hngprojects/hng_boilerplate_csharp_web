using Hng.Application.Features.Blogs.Dtos;
using MediatR;

namespace Hng.Application.Features.Blogs.Queries;

public class GetBlogByIdQuery(Guid id) : IRequest<BlogDto>
{
    public Guid BlogId { get; } = id;
}