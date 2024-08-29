using Hng.Application.Features.ContactsUs.Dtos;
using Hng.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hng.Application.Features.ContactsUs.Mappers
{
    public class ContactUsMappingProfile : AutoMapper.Profile
    {
        public ContactUsMappingProfile()
        {
            CreateMap<ContactUs, ContactUsResponseDto>().ReverseMap();
            CreateMap<ContactUsRequestDto, ContactUs>()
             .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName))
             .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
             .ReverseMap();
        }
    }
}
