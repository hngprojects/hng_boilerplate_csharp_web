using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Hng.Domain.Models;
using Hng.Web.Data;
using Hng.Web.Data.Dto;

namespace Hng.Web.Helper
{
    public class MappingProfile: AutoMapper.Profile
    {
          public MappingProfile()
        {
            CreateMap<Product, ProductDto>().ReverseMap();
            CreateMap<Domain.Models.Profile, ProfileForReturn>();
        }
    }
}