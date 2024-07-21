using Hng.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hng.Infrastructure.Repository.Interface
{
    public interface IJwtService
    {
        string GenerateToken(User user);
    }
}
