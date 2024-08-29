using Hng.Application.Features.Languages.Commands;
using Hng.Application.Features.Languages.Dtos;
using Hng.Domain.Entities;

namespace Hng.Application.Features.Languages.Mappers
{
    public class LanguageMapperProfile : AutoMapper.Profile
    {
        public LanguageMapperProfile()
        {
            CreateMap<Language, LanguageDto>().ReverseMap();
            CreateMap<CreateLanguageCommand, Language>();
            CreateMap<UpdateLanguageCommand, Language>();
        }
    }
}