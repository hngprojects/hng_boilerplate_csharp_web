using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using Hng.Infrastructure.Services.Interfaces;
using Hng.Infrastructure.Utilities;
using Microsoft.Extensions.Options;

namespace Hng.Infrastructure.Services;

public class OrganisationInviteService(IRepository<OrganizationInvite> repository, IOptions<FrontendUrl> options) : IOrganisationInviteService
{
    private readonly IRepository<OrganizationInvite> repository = repository;
    private readonly IOptions<FrontendUrl> options = options;

    public async Task<OrganizationInvite> CreateInvite(Guid userId, Guid orgId, string email)
    {
        var organizationInvite = new OrganizationInvite()
        {
            OrganizationId = orgId,
            Email = email,
            InviteLink = $"{options.Value.Path}/invite?{Guid.NewGuid()}"
        };

        await repository.AddAsync(organizationInvite);

        await repository.SaveChanges();

        return organizationInvite;
    }

}
