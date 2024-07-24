using Hng.Domain.Entities;

namespace Hng.Infrastructure.Services.Interfaces
{
    public interface IUserService
    {
        Task<(bool isUnique, string errorMessage)> IsEmailUniqueAsync(string email);
        string GenerateJwtToken(User user);
        string HashPassword(string password);
    }
}
