using Hng.Application.Features.Organisations.Dtos;
using Hng.Domain.Entities;

namespace Hng.Application.Features.Organisations.Mappers
{
    public class OrganizationMapperProfile : AutoMapper.Profile
    {
        public OrganizationMapperProfile()
        {
            CreateMap<Organization, OrganizationDto>()
                .ReverseMap();

            CreateMap<CreateOrganizationDto, Organization>();
        }
    }
}