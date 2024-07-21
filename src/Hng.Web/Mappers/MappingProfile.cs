using Hng.Application.Dto;
using Hng.Domain.Entities;

namespace Hng.Web.Mappers
{
    public class MappingProfile : AutoMapper.Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDto>()
                .ForMember(dest => dest.organizations, opt => opt.MapFrom(src => src.Organizations));

            CreateMap<UserDto, User>()
                .ForMember(dest => dest.Organizations, opt => opt.MapFrom(src => src.organizations));

            CreateMap<Product, ProductDto>().ReverseMap();
            CreateMap<Profile, ProfileDto>().ReverseMap();
            CreateMap<Organization, OrganizationDto>().ReverseMap();
        }
    }
}