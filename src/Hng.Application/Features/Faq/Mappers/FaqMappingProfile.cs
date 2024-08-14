using AutoMapper;
using Hng.Application.Features.Faq.Dtos;
using Hng.Domain.Entities;

public class FaqMappingProfile : AutoMapper.Profile
{
    public FaqMappingProfile()
    {
        CreateMap<Faq, FaqResponseDto>();
        // Mapping from Faq to CreateFaqResponseDto
        CreateMap<Faq, CreateFaqResponseDto>()
            .ForPath(dest => dest.Data.Id, opt => opt.MapFrom(src => src.Id))
            .ForPath(dest => dest.Data.Question, opt => opt.MapFrom(src => src.Question))
            .ForPath(dest => dest.Data.Answer, opt => opt.MapFrom(src => src.Answer))
            .ForPath(dest => dest.Data.Category, opt => opt.MapFrom(src => src.Category))
            .ForPath(dest => dest.Data.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
            .ForPath(dest => dest.Data.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt))
            .ForPath(dest => dest.Data.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy))
            .ReverseMap();

        // Mapping from CreateFaqRequestDto to Faq
        CreateMap<CreateFaqRequestDto, Faq>()
            .ForMember(dest => dest.Question, opt => opt.MapFrom(src => src.Question))
            .ForMember(dest => dest.Answer, opt => opt.MapFrom(src => src.Answer))
            .ReverseMap();

        // Mapping from Faq to UpdateFaqResponseDto
        CreateMap<Faq, UpdateFaqResponseDto>()
            .ForPath(dest => dest.Data.Id, opt => opt.MapFrom(src => src.Id))
            .ForPath(dest => dest.Data.Question, opt => opt.MapFrom(src => src.Question))
            .ForPath(dest => dest.Data.Answer, opt => opt.MapFrom(src => src.Answer))
            .ForPath(dest => dest.Data.Category, opt => opt.MapFrom(src => src.Category))
            .ReverseMap();

        // Mapping from UpdateFaqRequestDto to Faq
        CreateMap<UpdateFaqRequestDto, Faq>()
            .ForMember(dest => dest.Question, opt => opt.MapFrom(src => src.Question))
            .ForMember(dest => dest.Answer, opt => opt.MapFrom(src => src.Answer))
            .ReverseMap();

        // Mapping from Faq to FaqResponseDto
        CreateMap<Faq, FaqResponseDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Question, opt => opt.MapFrom(src => src.Question))
            .ForMember(dest => dest.Answer, opt => opt.MapFrom(src => src.Answer))
            .ReverseMap();
    }
}

