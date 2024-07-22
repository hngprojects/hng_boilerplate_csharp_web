using Hng.Application.Features.Organisations.Dtos;
using Hng.Domain.Entities;

namespace Hng.Application.Features.Organisations.Mappers
{
    public class OrganisationMapperProfile : AutoMapper.Profile
    {
        public OrganisationMapperProfile()
        {
            CreateMap<Organization, OrganizationDto>()
                .ReverseMap();
        }
    }
}