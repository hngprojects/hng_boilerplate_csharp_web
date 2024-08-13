using Hng.Application.Features.OrganisationInvite.Validators.ValidationErrors;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using Hng.Infrastructure.Utilities;
namespace Hng.Application.Features.OrganisationInvite.Validators;

public class RequestValidator : IRequestValidator
{
    public async Task<Result<Organization>> OrganizationExistsAsync(Guid orgId, IRepository<Organization> repository)
    {
        Organization org = await repository.GetAsync(orgId);
        return org == null ? OrganisationDoesNotExistError.FromId(orgId) : Result<Organization>.Success(org);
    }
    public async Task<Result<Organization>> UserIsOrganizationOwnerAsync(Guid userId, Guid orgId, IRepository<Organization> orgRepository)
    {


        Result<Organization> validOrgResult = await OrganizationExistsAsync(orgId, orgRepository);

        if (!validOrgResult.IsSuccess)
        {
            return validOrgResult;
        }

        return validOrgResult.Value.OwnerId == userId ?
        Result<Organization>.Success(validOrgResult.Value) : UserIsNotOwnerError.FromIds(userId, orgId);

    }
    public async Task<Result<OrganizationInvite>> InviteDoesNotExistAsync(
        Guid orgId,
        string inviteeEmail,
        IRepository<OrganizationInvite> inviteRepository
        )
    {

        OrganizationInvite uniqueInvite = await inviteRepository.GetBySpec(e => e.Email == inviteeEmail && e.OrganizationId == orgId);

        return uniqueInvite == null ?
        Result<OrganizationInvite>.Success(uniqueInvite) : InviteAlreadyExistsError.FromEmail(inviteeEmail);

    }
}
