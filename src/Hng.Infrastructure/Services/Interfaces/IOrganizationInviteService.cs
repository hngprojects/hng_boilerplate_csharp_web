using Hng.Domain.Common;
using Hng.Domain.Entities;

namespace Hng.Infrastructure.Services.Interfaces;

public interface IOrganizationInviteService
{
    public Task<Result<OrganizationInvite>> CreateInvite(Guid userId, Guid orgId, string email);
}
