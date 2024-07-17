using Hng.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hng.Domain.Repository
{
    public interface IWaitlistUserRepository : IGenericRepository<WaitlistUser>
    {
        Task<WaitlistUser> GetByEmailAsync(string email);
    }
}
