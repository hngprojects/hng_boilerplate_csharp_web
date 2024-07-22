using Hng.Domain.Entities;

namespace Hng.Infrastructure.Repository.Interface
{
    public interface IOrganisationRepository : IGenericRepository<Organization>
    {
        Task<Organization> AddOrganisation(Organization org);
    }
}