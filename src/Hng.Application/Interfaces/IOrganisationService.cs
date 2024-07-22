using Hng.Application.Dto;
using Hng.Domain.Entities;

namespace Hng.Application.Interfaces
{
    public interface IOrganisationService
    {
        Task<Organization> CreateOrganisationAsync(OrganizationDto org);
    }
}
