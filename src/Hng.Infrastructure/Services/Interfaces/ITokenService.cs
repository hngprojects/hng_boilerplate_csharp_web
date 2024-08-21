using Hng.Domain.Entities;

namespace Hng.Infrastructure.Services.Interfaces
{
    public interface ITokenService
    {
        public string GenerateJwt(User userData, int expireInMinutes = 0);

        public string GetCurrentUserEmail();

        public string GetForgotPasswordToken();
    }
}