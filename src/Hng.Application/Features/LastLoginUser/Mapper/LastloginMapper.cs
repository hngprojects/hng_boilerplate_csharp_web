using Hng.Application.Features.Faq.Dtos;
using Hng.Application.Features.LastLoginUser.Dto;
using Hng.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hng.Application.Features.LastLoginUser.Mapper
{
    public class LastloginMapper : AutoMapper.Profile
    {
        public LastloginMapper()
        {
            CreateMap<LastLogin, LastLoginDto>()
              .ReverseMap();
        }
    }
}
