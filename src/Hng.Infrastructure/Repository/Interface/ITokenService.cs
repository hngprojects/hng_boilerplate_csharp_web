using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hng.Infrastructure.Repository.Interface
{
    public interface ITokenService
    {
        string GenerateToken();
        Task StoreTokenAsync(string email, string token);
        Task<bool> ValidateTokenAsync(string email, string token);
    }

}
