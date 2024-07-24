using System.Security.Cryptography;
using System.Text;
using Hng.Infrastructure.Services.Interfaces;

namespace Hng.Infrastructure.Services
{
    public class PasswordService : IPasswordService
    {
        private const int SALTSIZE = 10;

        public (string passwordSalt, string hashedPassword) GeneratePasswordSaltAndHash(string plainPassword)
        {
            var buffer = RandomNumberGenerator.GetBytes(SALTSIZE);
            var salt = Convert.ToBase64String(buffer);
            string hashedPassword = GeneratePasswordHash(plainPassword, salt);
            return (salt, hashedPassword);
        }

        public bool IsPasswordEqual(string plainPassword, string passwordSalt, string passwordHash)
        {
            var newHash = GeneratePasswordHash(plainPassword, passwordSalt);
            return newHash == passwordHash;
        }
        private static string GeneratePasswordHash(string plainPassword, string salt)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(plainPassword + salt);
            var hash = SHA256.HashData(bytes);

            var hashedPassword = Convert.ToBase64String(hash);
            return hashedPassword;
        }
    }
}
