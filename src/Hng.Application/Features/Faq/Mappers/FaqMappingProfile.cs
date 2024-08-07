using AutoMapper;
using Hng.Application.Features.Faq.Dtos;
using Hng.Domain.Entities;

public class FaqMappingProfile : AutoMapper.Profile
{
    public FaqMappingProfile()
    {
        // Mapping from Faq to CreateFaqResponseDto
        CreateMap<Faq, CreateFaqResponseDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Question, opt => opt.MapFrom(src => src.Question))
            .ForMember(dest => dest.Answer, opt => opt.MapFrom(src => src.Answer))
            .ReverseMap();

        // Mapping from CreateFaqRequestDto to Faq
        CreateMap<CreateFaqRequestDto, Faq>()
            .ForMember(dest => dest.Question, opt => opt.MapFrom(src => src.Question))
            .ForMember(dest => dest.Answer, opt => opt.MapFrom(src => src.Answer))
            .ReverseMap();

        // Mapping from Faq to UpdateFaqResponseDto
        CreateMap<Faq, UpdateFaqResponseDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Question, opt => opt.MapFrom(src => src.Question))
            .ForMember(dest => dest.Answer, opt => opt.MapFrom(src => src.Answer))
            .ReverseMap();

        // Mapping from UpdateFaqRequestDto to Faq
        CreateMap<UpdateFaqRequestDto, Faq>()
            .ForMember(dest => dest.Question, opt => opt.MapFrom(src => src.Question))
            .ForMember(dest => dest.Answer, opt => opt.MapFrom(src => src.Answer))
            .ReverseMap();
    }
}

