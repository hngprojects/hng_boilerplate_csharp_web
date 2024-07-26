using System.Security.Cryptography;
using System.Text;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using Hng.Infrastructure.Services.Interfaces;

namespace Hng.Infrastructure.Services;

public class OrganizationInviteService(IRepository<Organization> Organizationrepository, IRepository<OrganizationInvite> repository) : IOrganizationInviteService
{
    private readonly IRepository<Organization> organizationrepository = Organizationrepository;
    private readonly IRepository<OrganizationInvite> repository = repository;

    public async Task<OrganizationInvite> CreateInvite(Guid userId, Guid orgId, string email)
    {
        Organization org = await organizationrepository.GetAsync(orgId);

        if (org == null)
        {
            return null;
        }

        if (await DoesInviteExist(email, orgId)) return null;

        if (org.OwnerId != userId) return null;

        var organizationInvite = new OrganizationInvite()
        {
            OrganizationId = orgId,
            Email = email,
            InviteLink = GenerateUniqueInviteLink(email)
        };

        await repository.AddAsync(organizationInvite);

        await repository.SaveChanges();

        return organizationInvite;
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
