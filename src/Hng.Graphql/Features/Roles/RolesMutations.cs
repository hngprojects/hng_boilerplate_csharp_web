using AutoMapper;
using Hng.Application.Features.Roles.Handler;
using Hng.Application.Features.Roles.Command;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using Hng.Application.Features.Roles.Dto;

namespace Hng.Graphql.Features.Roles
{
    public class RolesMutations
    {
        public async Task<CreateRoleResponseDto> CreateOrganisationRole(
            Guid orgId,
            CreateRoleRequestDto requestDto,
            [FromServices] IRepository<Organization> organizationRepository,
            [FromServices] IRepository<Role> roleRepository,
            [FromServices] IRepository<RolePermission> permissionsRepository,
            [FromServices] IMapper mapper)
        {
            var result = await new CreateRoleCommandHandler(organizationRepository, roleRepository, permissionsRepository, mapper)
                .Handle(new CreateRoleCommand(orgId, requestDto), CancellationToken.None);
            return result;
        }
    }
}
