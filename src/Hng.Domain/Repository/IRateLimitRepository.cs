using Hng.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hng.Domain.Repository
{
    public interface IRateLimitRepository : IGenericRepository<RateLimit>
    {
        Task<RateLimit> GetRateLimitByUserIdAsync(Guid userId);
        Task UpdateRateLimitAsync(RateLimit rateLimit);
        Task AddRateLimitAsync(RateLimit rateLimit);
    }
}
