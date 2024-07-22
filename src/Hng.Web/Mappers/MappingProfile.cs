using Hng.Application.Dto;
using Hng.Domain.Entities;

namespace Hng.Web.Mappers
{
    public class MappingProfile : AutoMapper.Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDto>()
                .ForMember(dest => dest.name, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"))
                .ReverseMap();

            CreateMap<Product, ProductDto>().ReverseMap();
            CreateMap<JobListing, JobListingDto>();
            CreateMap<CreateJobListingDto, JobListing>();

            CreateMap<Profile, ProfileDto>()
                .ForMember(dest => dest.first_name, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.last_name, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dest => dest.phone_number, opt => opt.MapFrom(src => src.PhoneNumber))
                .ForMember(dest => dest.avatar_url, opt => opt.MapFrom(src => src.AvatarUrl))
                .ReverseMap();

            CreateMap<Organization, OrganizationDto>()
                .ForMember(dest => dest.org_id, opt => opt.MapFrom(src => src.Id))
                .ReverseMap();
        }
    }
}
