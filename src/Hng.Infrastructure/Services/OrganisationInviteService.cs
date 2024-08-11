using System.Security.Cryptography;
using System.Text;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using Hng.Infrastructure.Services.Interfaces;
using Hng.Infrastructure.Utilities;
using Hng.Infrastructure.Utilities.Errors.OrganisationInvite;

namespace Hng.Infrastructure.Services;

public class OrganisationInviteService(IRepository<Organization> organizationRepository, IRepository<OrganizationInvite> repository) : IOrganisationInviteService
{
    private readonly IRepository<Organization> organizationRepository = organizationRepository;
    private readonly IRepository<OrganizationInvite> repository = repository;

    public async Task<Result<OrganizationInvite>> CreateInvite(Guid userId, Guid orgId, string email)
    {
        Organization org = await organizationRepository.GetAsync(orgId);

        if (org == null)
        {
            return OrganisationDoesNotExistError.FromId(orgId);
        }

        if (org.OwnerId != userId) return UserIsNotOwnerError.FromIds(userId, org.Id);

        if (await DoesInviteExist(email, orgId)) return InviteAlreadyExistsError.FromEmail(email);

        var organizationInvite = new OrganizationInvite()
        {
            OrganizationId = orgId,
            Email = email,
            InviteLink = GenerateUniqueInviteLink(email)
        };

        await repository.AddAsync(organizationInvite);

        await repository.SaveChanges();

        return Result<OrganizationInvite>.Success(organizationInvite);
    }

    private static string GenerateUniqueInviteLink(string email)
    {
        var now = DateTime.UtcNow;

        string input = $"{email}_{now:yyyy-MM-dd-HH-mm-ss}";
        byte[] bytes = SHA256.HashData(Encoding.UTF8.GetBytes(input));

        string base64Hash = Convert.ToBase64String(bytes)
            .Replace("+", "")
            .Replace("/", "")
            .Replace("=", "");

        string inviteLink = base64Hash[..16];

        return inviteLink;
    }

    private async Task<bool> DoesInviteExist(string email, Guid orgId)
    {
        var invite = await repository.GetBySpec(e => e.Email == email && e.OrganizationId == orgId);
        if (invite == null)
        {
            return false;
        }
        return true;
    }


}
