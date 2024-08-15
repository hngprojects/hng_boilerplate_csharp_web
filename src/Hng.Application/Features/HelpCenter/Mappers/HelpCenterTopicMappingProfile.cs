using Hng.Application.Features.HelpCenter.Command;
using Hng.Application.Features.HelpCenter.Dtos;
using Hng.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hng.Application.Features.HelpCenter.Mappers
{
    public class HelpCenterTopicMappingProfile : AutoMapper.Profile
    {
        public HelpCenterTopicMappingProfile()
        {

            CreateMap<CreateHelpCenterTopicCommand, HelpCenterTopic>()
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Request.Title))
                .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.Request.Content));

            CreateMap<UpdateHelpCenterTopicCommand, HelpCenterTopic>()
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Request.Title))
                .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.Request.Content))
                .ForMember(dest => dest.Author, opt => opt.MapFrom(src => src.Request.Author));


            CreateMap<HelpCenterTopic, HelpCenterTopicResponseDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.Content))
                .ReverseMap();


            CreateMap<SearchHelpCenterTopicsRequestDto, HelpCenterTopic>()
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.Content))
                .ReverseMap();

            CreateMap<UpdateHelpCenterTopicRequestDto, HelpCenterTopic>()
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
            .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.Content))
            .ForMember(dest => dest.Author, opt => opt.MapFrom(src => src.Author));
        }
    }
}
