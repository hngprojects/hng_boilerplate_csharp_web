using Hng.Application.Features.Blogs.Dtos;
using Hng.Domain.Entities;
using Profile = AutoMapper.Profile;

namespace Hng.Application.Features.Blogs.Mappers;

public class BlogMapperProfile : Profile
{
    public BlogMapperProfile()
    {
        CreateMap<CreateBlogDto, Blog>();
        CreateMap<Blog, BlogDto>()
            .ReverseMap();
    }
}