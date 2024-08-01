using AutoMapper;
using Hng.Application.Features.UserManagement.Dtos;
using Hng.Domain.Entities;

namespace Hng.Application.Features.UserManagement.Mappers
{
    public class UserMappingProfile : AutoMapper.Profile
    {
        public UserMappingProfile()
        {
            CreateMap<User, UserDto>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"))
                .ReverseMap();

            CreateMap<UserSignUpDto, User>()
                .ReverseMap();

            CreateMap<User, UserResponseDto>()
                .ReverseMap();
        }
    }
}