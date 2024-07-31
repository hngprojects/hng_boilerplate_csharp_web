using Hng.Domain.Entities;

namespace Hng.Infrastructure.Services.Interfaces;

public interface IAuthenticationService
{
    Task<User> GetCurrentUserAsync();
}