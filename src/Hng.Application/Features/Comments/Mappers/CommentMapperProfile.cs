using Hng.Application.Features.Comments.Dtos;
using Hng.Domain.Entities;
using Profile = AutoMapper.Profile;

namespace Hng.Application.Features.Comments.Mappers;

public class CommentMapperProfile : Profile
{
    public CommentMapperProfile()
    {
        CreateMap<CreateCommentDto, Comment>();
        CreateMap<Comment, CommentDto>()
            .ReverseMap();
    }
    
}