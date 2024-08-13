using AutoMapper;
using Hng.Domain.Entities;

namespace Hng.Application.Features.EmailTemplates.DTOs;

public class EmailTemplateMapperProfile : AutoMapper.Profile
{
    public EmailTemplateMapperProfile()
    {
        CreateMap<EmailTemplate, EmailTemplateDTO>().ReverseMap();

        CreateMap<CreateEmailTemplateDTO, EmailTemplate>().ReverseMap();
    }
}
