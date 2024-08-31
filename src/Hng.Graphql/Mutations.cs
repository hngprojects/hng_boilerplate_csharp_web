using Hng.Application.Features.Roles.Command;
using Hng.Application.Features.Roles.Dto;
using HotChocolate.Authorization;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Hng.Graphql
{
    public partial class Mutations
    {


        [Authorize]
        public async Task<CreateRoleResponseDto> CreateRoleInOrganisation(Guid orgId, CreateRoleRequestDto request, [FromServices] IMediator mediator)
        {
            CreateRoleCommand command = new CreateRoleCommand(orgId, request);
            return await mediator.Send(command);
        }


    }
}