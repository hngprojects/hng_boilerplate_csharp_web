using Hng.Application.Features.Roles.Command;
using Hng.Application.Features.Roles.Dto;
using Hng.Application.Features.UserManagement.Commands;
using Hng.Application.Features.UserManagement.Dtos;
using HotChocolate.Authorization;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Hng.Graphql
{
    public partial class Mutations
    {
        public async Task<UserLoginResponseDto<SignupResponseData>> Login(UserLoginRequestDto loginRequest, [FromServices] IMediator mediator)
        {
            var command = new CreateUserLoginCommand(loginRequest);
            return await mediator.Send(command);
        }

        public async Task<UserLoginResponseDto<SignupResponseData>> GoogleLogin(GoogleLoginRequestDto googleLoginRequest, [FromServices] IMediator mediator)
        {
            var command = new GoogleLoginCommand(googleLoginRequest.IdToken);
            return await mediator.Send(command);
        }

        [Authorize]
        public async Task<CreateRoleResponseDto> CreateRoleInOrganisation(Guid orgId, CreateRoleRequestDto request, [FromServices] IMediator mediator)
        {
            CreateRoleCommand command = new CreateRoleCommand(orgId, request);
            return await mediator.Send(command);
        }


    }
}