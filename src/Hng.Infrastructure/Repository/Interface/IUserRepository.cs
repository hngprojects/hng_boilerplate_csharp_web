using Hng.Domain.Entities;

namespace Hng.Infrastructure.Repository.Interface
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User> GetUserById(Guid id);
        Task<IEnumerable<User>> GetUsersByStatusAsync(string org_id, string status, int page, int limit);
    }
}
