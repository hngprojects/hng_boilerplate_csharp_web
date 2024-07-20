using Hng.Domain.Entities.Models;
using Hng.Infrastructure.Context;

namespace Hng.Infrastructure.Repository
{
    public class UserRepository(MyDBContext context) : IRepository<User>(context)
    {
    }
}
