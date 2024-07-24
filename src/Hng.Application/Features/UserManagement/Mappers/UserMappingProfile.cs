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
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"))
                .ReverseMap();

            // mapping for UserSignUpDto to User
            CreateMap<UserSignUpDto, User>()
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore()); // Password should be hashed separately

            // mapping for User to UserResponseDto
            CreateMap<User, UserResponseDto>()
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"));
        }

    }
    
}