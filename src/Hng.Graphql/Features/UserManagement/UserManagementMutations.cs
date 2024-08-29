using AutoMapper;
using Hng.Application.Features.UserManagement.Commands;
using Hng.Application.Features.UserManagement.Dtos;
using Hng.Application.Features.UserManagement.Handlers;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using Hng.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Hng.Graphql.Features.UserManagement
{
    public class UserManagementMutations
    {
        public async Task<UserLoginResponseDto<SignupResponseData>> LoginUser(
            UserLoginRequestDto loginDetails,
            [FromServices] IRepository<User> userRepo,
            [FromServices] IRepository<LastLogin> loginLast,
            [FromServices] IMapper mapper,
            [FromServices] IPasswordService passwordService,
            [FromServices] ITokenService tokenService,
            [FromServices] IHttpContextAccessor httpContextAccessor)
        {

            var result = await new LoginUserCommandHandler(userRepo, loginLast, mapper, passwordService, tokenService, httpContextAccessor)
            .Handle(new CreateUserLoginCommand(loginDetails), CancellationToken.None);
            return result;
        }

        public async Task<UserLoginResponseDto<SignupResponseData>> GoogleLoginUser(GoogleLoginRequestDto loginRequest,
            [FromServices] IRepository<User> userRepo,
            [FromServices] IRepository<Role> roleRepository,
            [FromServices] ITokenService tokenService,
            [FromServices] IMapper mapper,
            [FromServices] IGoogleAuthService googleAuthService)
        {
            var result = await new GoogleLoginCommandHandler(userRepo, roleRepository, tokenService, mapper, googleAuthService)
                .Handle(
                    new GoogleLoginCommand(loginRequest.IdToken),
                    CancellationToken.None);
            return result;
        }
    }
}
