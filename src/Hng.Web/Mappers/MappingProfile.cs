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

            // Updated mapping for CreateSubscriptionPlanDto to SubscriptionPlan
            CreateMap<CreateSubscriptionPlanDto, SubscriptionPlan>()
                .ForMember(dest => dest.Features, opt => opt.MapFrom(src => src.Features.Select(f => new Feature { Name = f })));

            CreateMap<SubscriptionPlan, SubscriptionPlanResponse>()
                .ForMember(dest => dest.Features, opt => opt.MapFrom(src => src.Features.Select(f => f.Name)));
        }
    }
}