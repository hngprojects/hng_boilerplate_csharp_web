using Hng.Domain.Entities;
using Hng.Domain.Repositories;
using Hng.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hng.Domain.Repository
{
    public class WaitlistUserRepository : GenericRepository<WaitlistUser>, IWaitlistUserRepository
    {
        public WaitlistUserRepository(MyDBContext context) : base(context)
        {
        }

        public async Task<WaitlistUser> GetByEmailAsync(string email)
        {
            return await _context.Set<WaitlistUser>()
                .FirstOrDefaultAsync(u => u.Email == email);
        }
    }
}
