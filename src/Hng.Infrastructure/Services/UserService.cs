using Hng.Domain.Entities.Models;
using Hng.Infrastructure.Repository;

namespace Hng.Infrastructure.Services
{
    public class UserService(UserRepository userRepository):IUserService
    {
        public async Task<List<User>> GetUsers()
        {
            var users = userRepository.Get();
            return users.ToList();
        }
    }

    public interface IUserService
    {
        Task<List<User>> GetUsers();
    }
}
