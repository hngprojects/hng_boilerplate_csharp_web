
namespace Hng.Infrastructure.Services.Interfaces;

public interface IAuthenticationService
{
    Task<Guid> GetCurrentUserAsync();
}