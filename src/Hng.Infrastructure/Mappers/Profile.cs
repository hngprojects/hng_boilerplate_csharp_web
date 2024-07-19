using Hng.Application.Dto;
using Hng.Domain.Models;

namespace Hng.Web.Helper
{
    public class Profile : AutoMapper.Profile
    {
        public Profile()
        {
            CreateMap<Product, ProductDto>().ReverseMap();
            CreateMap<Profile, ProfileDto>().ReverseMap();
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<Organisation, OrganisationDto>().ReverseMap();
        }
    }
}