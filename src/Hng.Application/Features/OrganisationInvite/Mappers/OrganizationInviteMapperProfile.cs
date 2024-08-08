using Hng.Application.Features.OrganisationInvite.Dtos;
using Hng.Domain.Entities;

namespace Hng.Application.Features.OrganisationInvite.Mappers;

public class OrganizationInviteMapperProfile : AutoMapper.Profile
{
    public OrganizationInviteMapperProfile()
    {
        CreateMap<OrganizationInvite, OrganizationInviteDto>();
    }
}
