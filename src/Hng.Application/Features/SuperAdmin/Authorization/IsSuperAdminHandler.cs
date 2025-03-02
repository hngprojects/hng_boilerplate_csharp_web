using Hng.Application.Shared.Dtos;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using Hng.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace Hng.Application.Features.SuperAdmin.Authorization;

public class IsSuperAdminHandler : AuthorizationHandler<IsSuperAdminRequirement>
{
    private readonly IRepository<User> _userRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IAuthenticationService _authenticationService;

    public IsSuperAdminHandler(IRepository<User> userRepository, IHttpContextAccessor httpContextAccessor, IAuthenticationService authenticationService)
    {
        _userRepository = userRepository;
        _httpContextAccessor = httpContextAccessor;
        _authenticationService = authenticationService;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, IsSuperAdminRequirement requirement)
    {

        var userId = await _authenticationService.GetCurrentUserAsync();

        if (!Guid.TryParse(userId.ToString(), out _))
        {
            context.Fail();
            return;
        }

        var user = await _userRepository.GetBySpec(u => u.Id == userId);

        Console.WriteLine($"This is the User : {user}");

        if (user != null && user.IsSuperAdmin)
        {
            context.Succeed(requirement);
        }
        else
        {
            var response = new StatusCodeResponse()
            {
                StatusCode = StatusCodes.Status403Forbidden,
                Message = "You are not Authorized to access this resource"
            };
            _httpContextAccessor.HttpContext.Response.StatusCode = StatusCodes.Status403Forbidden;
            await _httpContextAccessor.HttpContext.Response.WriteAsJsonAsync(response);
            context.Fail();
        }
    }
}
