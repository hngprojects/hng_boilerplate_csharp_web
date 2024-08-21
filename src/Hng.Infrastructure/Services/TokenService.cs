using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Hng.Domain.Entities;
using Hng.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Hng.Infrastructure.Utilities;

namespace Hng.Infrastructure.Services
{
    public class TokenService(IHttpContextAccessor context, Jwt jwtKeys) : ITokenService
    {
        private readonly IHttpContextAccessor _context = context;
        private readonly Jwt _jwtKeys = jwtKeys;

        public static TokenValidationParameters GetTokenValidationParameters(string secretKey) => new()
        {
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = GetSecurityKey(secretKey),
        };

        public string GenerateJwt(User userData, int expireInMinutes = 0)
        {
            SymmetricSecurityKey securityKey = GetSecurityKey(_jwtKeys.SecretKey);

            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            Claim[] claims = [
                new(ClaimTypes.Sid, userData.Id.ToString()),
                new(ClaimTypes.Email, userData.Email),
                new(ClaimTypes.Name, userData.FirstName),
                new(ClaimTypes.NameIdentifier,
                !string.IsNullOrWhiteSpace(userData.PasswordResetToken) ? userData.PasswordResetToken : "")
                ];

            expireInMinutes = expireInMinutes == 0 ? _jwtKeys.ExpireInMinute : expireInMinutes;
            var tokenObject = new JwtSecurityToken(
                claims: claims,
                signingCredentials: signingCredentials,
                expires: DateTime.UtcNow.AddMinutes(expireInMinutes)
                );

            return new JwtSecurityTokenHandler().WriteToken(tokenObject);
        }

        public string GetCurrentUserEmail()
        {
            var identity = _context.HttpContext.User.Identity as ClaimsIdentity;

            // Gets list of claims
            var claim = identity.Claims;

            // Gets user email from claims. Generally it's a  string.
            var loggedInUSerEmail = claim
                .First(x => x.Type == ClaimTypes.Email).Value;

            return loggedInUSerEmail;
        }

        public string GetForgotPasswordToken()
        {
            var identity = _context.HttpContext.User.Identity as ClaimsIdentity;

            var claim = identity.Claims;

            var forgotPasswordToken = claim
                .First(x => x.Type == ClaimTypes.NameIdentifier).Value;

            return forgotPasswordToken;
        }

        private static SymmetricSecurityKey GetSecurityKey(string secretKey)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!));
            return securityKey;
        }
    }
}
