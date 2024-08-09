using AutoMapper;
using Hng.Application.Features.Timezones.Commands;
using Hng.Application.Features.Timezones.Dtos;
using Hng.Domain.Entities;

namespace Hng.Application.Features.Timezones.Mappers
{
    public class TimezoneMapperProfile : AutoMapper.Profile
    {
        public TimezoneMapperProfile()
        {
            CreateMap<Timezone, TimezoneDto>()
                .ForMember(dest => dest.Timezone, opt => opt.MapFrom(src => src.TimezoneValue))
                .ReverseMap();

            CreateMap<CreateTimezoneCommand, Timezone>()
                .ForMember(dest => dest.TimezoneValue, opt => opt.MapFrom(src => src.Timezone));
        }
    }
}