using AutoMapper;
using Google.Apis.Auth;
using Hng.Application.Features.Organisations.Dtos;
using Hng.Application.Features.UserManagement.Dtos;
using Hng.Domain.Entities;


namespace Hng.Application.Features.UserManagement.Mappers
{
    public class UserMappingProfile : AutoMapper.Profile
    {
        public UserMappingProfile()
        {
            CreateMap<GoogleJsonWebSignature.Payload, User>()
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.GivenName));
            CreateMap<User, UserDto>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"))
                .ForMember(dest => dest.Organizations, opt => opt.MapFrom(src => src.Organizations))
                .ReverseMap();

            CreateMap<UserSignUpDto, User>()
                .ReverseMap();


            CreateMap<User, UserResponseDto>()
                .ReverseMap();
        }
    }
}