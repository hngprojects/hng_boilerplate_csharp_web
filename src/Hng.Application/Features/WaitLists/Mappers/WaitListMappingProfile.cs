using Hng.Application.Features.WaitLists.Dtos;
using Hng.Domain.Entities;

namespace Hng.Application.Features.WaitLists.Mappers
{
    internal class WaitListMappingProfile : AutoMapper.Profile
    {
        public WaitListMappingProfile()
        {
            CreateMap<Waitlist, WaitListDto>().ReverseMap();
        }
    }
}
