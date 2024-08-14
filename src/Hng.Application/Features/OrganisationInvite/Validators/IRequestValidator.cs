using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using Hng.Infrastructure.Utilities;

namespace Hng.Application.Features.OrganisationInvite.Validators;

public interface IRequestValidator
{
    public Task<Result<Organization>> OrganizationExistsAsync(Guid orgId, IRepository<Organization> repository);
    public Task<Result<Organization>> UserIsOrganizationOwnerAsync(Guid userId, Guid orgId, IRepository<Organization> orgRepository);
    public Task<Result<OrganizationInvite>> InviteDoesNotExistAsync(
        Guid orgId,
        string inviteeEmail,
        IRepository<OrganizationInvite> inviteRepository);
}
