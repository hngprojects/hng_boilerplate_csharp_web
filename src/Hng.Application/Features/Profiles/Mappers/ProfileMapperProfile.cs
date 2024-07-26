using Hng.Application.Features.PaymentIntegrations.Paystack.Dtos.Requests;
using Hng.Application.Features.Profiles.Dtos;
using Hng.Domain.Entities;

namespace Hng.Application.Features.Profiles.Mappers
{
    public class ProfileMapperProfile : AutoMapper.Profile
    {
        public ProfileMapperProfile()
        {
            CreateMap<Profile, ProfileDto>()
                .ForMember(dest => dest.first_name, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.last_name, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dest => dest.phone_number, opt => opt.MapFrom(src => src.PhoneNumber))
                .ForMember(dest => dest.avatar_url, opt => opt.MapFrom(src => src.AvatarUrl))
                .ReverseMap();

            CreateMap<Transaction, InitializeTransactionCommand>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User.Email))
                .ReverseMap();
        }
    }
}