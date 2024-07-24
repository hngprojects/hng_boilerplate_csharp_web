using Hng.Domain.Entities;

namespace Hng.Infrastructure.Services.Interfaces;

public interface IOrganizationInviteService
{
    public Task<OrganizationInvite> CreateInvite(Guid userId, Guid orgId, string email);
    
}
