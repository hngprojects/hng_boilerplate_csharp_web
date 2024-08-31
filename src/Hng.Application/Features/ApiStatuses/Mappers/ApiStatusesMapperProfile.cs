using Hng.Application.Features.ApiStatuses.Dtos.Requests;
using Hng.Application.Features.ApiStatuses.Dtos.Responses;
using Hng.Domain.Entities;

namespace Hng.Application.Features.ApiStatuses.Mappers
{
    public class ApiStatusesMapperProfile : AutoMapper.Profile
    {
        public ApiStatusesMapperProfile()
        {
            // Map from DTO to Entity
            CreateMap<ApiStatus, ApiStatusResponseDto>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
            .ForMember(dest => dest.Details, opt => opt.MapFrom(src => src.Details))
            .ForMember(dest => dest.ApiGroup, opt => opt.MapFrom(src => src.ApiGroup))
            .ForMember(dest => dest.LastChecked, opt => opt.MapFrom(src => src.LastChecked))
            .ForMember(dest => dest.ResponseTime, opt => opt.MapFrom(src => src.ResponseTime))
                .ReverseMap();

            // Map from Entity to Response DTO
            CreateMap<ApiStatusModel, ApiStatus>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
            .ForMember(dest => dest.Details, opt => opt.MapFrom(src => src.Details))
            .ForMember(dest => dest.ApiGroup, opt => opt.MapFrom(src => src.ApiGroup))
            .ForMember(dest => dest.ResponseTime, opt => opt.MapFrom(src => src.ResponseTime))
            .ReverseMap();
        }
    }
}