using Hng.Application.Features.Roles.Dto;
using Hng.Application.Features.Roles.Queries;
using Hng.Application.Features.UserManagement.Dtos;
using Hng.Application.Features.UserManagement.Queries;
using HotChocolate.Authorization;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Hng.Graphql
{
    public class Queries
    {
        [Authorize]
        public async Task<UserDto> GetLoggedInUsersDetails([FromServices] IMediator mediator)
        {
            var query = new GetLoggedInUserDetailsQuery();
            return await mediator.Send(query);
        }

        [Authorize]
        public async Task<IEnumerable<RoleDto>> GetRolesInOrganisation(Guid orgId, [FromServices] IMediator mediator)
        {
            var query = new GetRolesQuery(orgId);
            return await mediator.Send(query);
        }

        [Authorize]
        public async Task<IEnumerable<PermissionDto>> GetAllPermissions([FromServices] IMediator mediator)
        {
            var query = new GetRolePermissionsQuery();
            return await mediator.Send(query);
        }
    }
}
