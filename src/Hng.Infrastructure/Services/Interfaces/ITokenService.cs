using Hng.Domain.Entities;

namespace Hng.Infrastructure.Services.Interfaces
{
    public interface ITokenService
    {
        public string GenerateJwt(User userData);

        public string GetCurrentUserEmail();
    }
}