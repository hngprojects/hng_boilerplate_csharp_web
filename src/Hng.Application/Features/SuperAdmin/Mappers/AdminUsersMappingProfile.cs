using Hng.Application.Features.SuperAdmin.Dto;
using Hng.Domain.Entities;

namespace Hng.Application.Features.SuperAdmin.Mappers
{
    public class AdminUsersMappingProfile : AutoMapper.Profile
    {
        public AdminUsersMappingProfile()
        {
            CreateMap<User, UserSuperDto>().ReverseMap();
        }
    }
}
