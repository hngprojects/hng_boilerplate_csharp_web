using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Hng.Application.Services
{
    public class TokenService
    {
        private readonly ConcurrentDictionary<string, (string Token, DateTime Expiry)> _tokens = new();
        private readonly TimeSpan _tokenExpiry = TimeSpan.FromMinutes(5);

        public string GenerateToken()
        {
            using var rng = new RNGCryptoServiceProvider();
            var tokenData = new byte[4];
            rng.GetBytes(tokenData);
            var token = BitConverter.ToUInt32(tokenData, 0) % 1000000; // 6-digit token
            return token.ToString("D6");
        }

        public void StoreToken(string email, string token)
        {
            var expiry = DateTime.UtcNow.Add(_tokenExpiry);
            _tokens[email] = (token, expiry);
        }

        public bool ValidateToken(string email, string token)
        {
            if (_tokens.TryGetValue(email, out var storedToken))
            {
                if (storedToken.Token == token && storedToken.Expiry > DateTime.UtcNow)
                {
                    _tokens.TryRemove(email, out _);
                    return true;
                }
            }
            return false;
        }
    }
}
