using System.Security.Claims;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using Hng.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Hng.Infrastructure.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IRepository<User> _userRepository;

    public AuthenticationService(IHttpContextAccessor httpContextAccessor, IRepository<User> userRepository)
    {
        _httpContextAccessor = httpContextAccessor;
        _userRepository = userRepository;
    }

    public async Task<User> GetCurrentUserAsync()
    {
        var identity = _httpContextAccessor.HttpContext?.User.Identity as ClaimsIdentity;

        var userId = identity?.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
        {
            return null;
        }

        var userIdGuid = Guid.Parse(userId);
        var user = await _userRepository.GetBySpec(u => u.Id == userIdGuid);
        return user;
    }
}