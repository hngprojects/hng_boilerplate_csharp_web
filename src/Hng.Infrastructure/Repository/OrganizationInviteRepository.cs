using Hng.Domain.Entities;
using Hng.Infrastructure.Context;

namespace Hng.Infrastructure.Repository.Interface;

public class OrganizationInviteRepository(ApplicationDbContext context) : Repository<OrganizationInvite>(context), IOrganizationInviteRepository
{
}
