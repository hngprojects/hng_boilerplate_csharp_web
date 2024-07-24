using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hng.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Hng.Infrastructure.Services.Interfaces
{
    public interface ITokenService
    {
        public string GenerateJwt(User userData);
    }
}