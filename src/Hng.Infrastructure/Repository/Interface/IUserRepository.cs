using Hng.Domain.Entities;

namespace Hng.Infrastructure.Repository.Interface
{
    public interface IUserRepository : IGenericRepository<User>
    {
        //object Users { get; set; }

        Task<User> GetUserById(Guid id);


    }
}