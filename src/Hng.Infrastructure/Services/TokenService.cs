using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Hng.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Hng.Infrastructure.Services.Interfaces;

namespace Hng.Infrastructure.Services
{
    public class TokenService(IConfiguration config) : ITokenService
    {
        private readonly IConfiguration _config = config;

        public static TokenValidationParameters GetTokenValidationParameters(IConfiguration _config) => new TokenValidationParameters
        {
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = GetSecurityKey(_config),
        };

        public string GenerateJwt(User userData)
        {
            SymmetricSecurityKey securityKey = GetSecurityKey(_config);

            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            Claim[] claims = [
                new(ClaimTypes.Sid, userData.Id.ToString()),
                new(ClaimTypes.Email, userData.Email),
                new(ClaimTypes.Name, userData.FirstName)
                ];

            var expireInMinutes = Convert.ToInt32(_config["Jwt:ExpireInMinute"]);
            var tokenObject = new JwtSecurityToken(
                claims: claims,
                signingCredentials: signingCredentials,
                expires: DateTime.Now.AddMinutes(expireInMinutes)
                );

            return new JwtSecurityTokenHandler().WriteToken(tokenObject);
        }

        private static SymmetricSecurityKey GetSecurityKey(IConfiguration _config)
        {
            var secretKey = _config["Jwt:SecretKey"];
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!));
            return securityKey;
        }
    }
}
