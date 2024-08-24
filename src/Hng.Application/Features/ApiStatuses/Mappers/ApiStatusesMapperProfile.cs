using Hng.Application.Features.ApiStatuses.Dtos.Responses;
using Hng.Application.Features.Timezones.Commands;
using Hng.Application.Features.Timezones.Dtos;
using Hng.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hng.Application.Features.ApiStatuses.Mappers
{
    public class ApiStatusesMapperProfile : AutoMapper.Profile
    {
        public ApiStatusesMapperProfile()
        {
            CreateMap<ApiStatus, ApiStatusResponseDto>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
            .ForMember(dest => dest.Details, opt => opt.MapFrom(src => src.Details))
            .ForMember(dest => dest.ApiGroup, opt => opt.MapFrom(src => src.ApiGroup))
            .ForMember(dest => dest.LastChecked, opt => opt.MapFrom(src => src.LastChecked))
            .ForMember(dest => dest.ResponseTime, opt => opt.MapFrom(src => src.ResponseTime))
                .ReverseMap();
        }
    }
}
