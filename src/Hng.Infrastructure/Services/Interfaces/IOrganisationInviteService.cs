using Hng.Domain.Entities;
using Hng.Infrastructure.Utilities;

namespace Hng.Infrastructure.Services.Interfaces;

public interface IOrganisationInviteService
{
    public Task<OrganizationInvite> CreateInvite(Guid userId, Guid orgId, string email);
}
