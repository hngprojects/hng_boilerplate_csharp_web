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
    public class RateLimitRepository : GenericRepository<RateLimit>, IRateLimitRepository
    {
        public RateLimitRepository(MyDBContext context) : base(context)
        {
        }

        public async Task<RateLimit> GetRateLimitByUserIdAsync(Guid userId)
        {
            return await _context.Set<RateLimit>()
                .FirstOrDefaultAsync(r => r.UserId == userId.ToString());
        }

        public async Task UpdateRateLimitAsync(RateLimit rateLimit)
        {
            _context.Entry(rateLimit).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task AddRateLimitAsync(RateLimit rateLimit)
        {
            await _context.Set<RateLimit>().AddAsync(rateLimit);
            await _context.SaveChangesAsync();
        }
    }
}
