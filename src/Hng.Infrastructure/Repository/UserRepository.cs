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
                .Include(x => x.Organizations)
                .FirstOrDefaultAsync(x => x.Id == id) ?? throw new KeyNotFoundException($"User with ID {id} was not found.");
            return user;
        }

        public override async Task<ICollection<User>> GetAllAsync()
        {
            var users = await _context.Users
            .Include(u => u.Profile)
            .Include(u => u.Products)
            .Include(u => u.Organizations)
            .ToListAsync();
            return users;
        }

        public async Task<IEnumerable<User>> GetUsersByStatusAsync(string org_id, string status, int page, int limit)
        {
            return await _context.Users
            .Where(u => u.Org_id == org_id && u.Status == status)
            .Skip((page - 1) * limit)
            .Take(limit)
            .ToListAsync();
        }
    }
}
