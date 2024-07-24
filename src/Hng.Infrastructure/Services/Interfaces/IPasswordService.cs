using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hng.Infrastructure.Services.Interfaces
{
    public interface IPasswordService
    {
        public (string passwordSalt, string hashedPassword) GeneratePasswordSaltAndHash(string plainPassword);
        public bool IsPasswordEqual(string plainPassword, string passwordSalt, string passwordHash);
    }
}