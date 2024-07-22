using Hng.Domain.Entities;
using Hng.Infrastructure.Context;
using Hng.Infrastructure.Repository.Interface;

namespace Hng.Infrastructure.Repository
{
    public class OrganisationRepository : GenericRepository<Organization>, IOrganisationRepository
    {
        private readonly MyDBContext _context;

        public OrganisationRepository(MyDBContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Organization> AddOrganisation(Organization org)
        {
            var organisation = new Organization
            {
                Id = Guid.NewGuid(),
                Name = org.Name,
                Description = org.Description
            };

            _context.Organizations.Add(organisation);
            await _context.SaveChangesAsync();
            return organisation;
        }
    }
}
