using Hng.Application.Features.Blogs.Dtos;
using MediatR;

namespace Hng.Application.Features.Blogs.Queries;

public class GetBlogsQuery : IRequest<IEnumerable<BlogDto>>;
