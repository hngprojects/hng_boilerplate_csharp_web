using Hng.Application.Dto;
using Hng.Domain.Entities;

namespace Hng.Web.Mappers
{
    public class Profile : AutoMapper.Profile
    {
        public Profile()
        {
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<Product, ProductDto>().ReverseMap();
            CreateMap<Profile, ProfileDto>().ReverseMap();
            CreateMap<Organisation, OrganisationDto>().ReverseMap();
        }
    }
}