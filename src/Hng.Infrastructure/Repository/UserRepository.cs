using Hng.Domain.Entities;
using Hng.Infrastructure.Context;
using Hng.Infrastructure.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace Hng.Infrastructure.Repository
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        private readonly MyDBContext _context;

        public UserRepository(MyDBContext context) : base(context)
        {
            _context = context;
        }

        public async Task<User> GetUserById(Guid id)
        {

            var user = await _context.Users
                .Include(x => x.Products)
                .Include(x => x.Profile)
                .Include(x => x.Organisations)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (user == null)
            {
                throw new KeyNotFoundException($"User with ID {id} was not found.");
            }

            return user;
        }
    }
}
