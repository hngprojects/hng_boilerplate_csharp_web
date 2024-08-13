using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using Hng.Infrastructure.Services.Interfaces;

namespace Hng.Infrastructure.Services;

public class OrganisationInviteService(IRepository<OrganizationInvite> repository, IMessageQueueService queueService) : IOrganisationInviteService
{
    private readonly IRepository<OrganizationInvite> repository = repository;
    public async Task<OrganizationInvite> CreateInvite(Guid userId, Guid orgId, string email)
    {
        var organizationInvite = new OrganizationInvite()
        {
            OrganizationId = orgId,
            Email = email,
            InviteLink = Guid.NewGuid()
        };

        await repository.AddAsync(organizationInvite);

        await repository.SaveChanges();

        return organizationInvite;
    }

}
