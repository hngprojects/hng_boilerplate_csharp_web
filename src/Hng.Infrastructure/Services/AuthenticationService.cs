using System.Security.Claims;
using Hng.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Hng.Infrastructure.Services;

public class AuthenticationService(IHttpContextAccessor httpContextAccessor) : IAuthenticationService
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public Task<Guid> GetCurrentUserAsync()
    {
        if (_httpContextAccessor.HttpContext.User.Identity is not ClaimsIdentity identity)
            throw new InvalidOperationException("User identity is not available.");

        var claim = identity.Claims;

        var loggedInUserId = claim.FirstOrDefault(x => x.Type == ClaimTypes.Sid)?.Value;

        if (string.IsNullOrEmpty(loggedInUserId))
            throw new InvalidOperationException("User ID is not available in the claims.");

        if (!Guid.TryParse(loggedInUserId, out var userId))
            throw new InvalidOperationException("Invalid user ID format.");

        return Task.FromResult(userId);
    }


}