using Hng.Application.Features.ExternalIntegrations.PaymentIntegrations.Paystack.Dtos.Requests;
using Hng.Application.Features.Profiles.Dtos;
using Hng.Domain.Entities;

namespace Hng.Application.Features.Profiles.Mappers
{
    public class ProfileMapperProfile : AutoMapper.Profile
    {
        public ProfileMapperProfile()
        {
            CreateMap<Profile, ProfileDto>().ReverseMap();

            CreateMap<Transaction, InitializeTransactionCommand>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User.Email))
                .ReverseMap();
        }
    }
}