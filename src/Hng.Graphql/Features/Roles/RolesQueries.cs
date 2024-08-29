using AutoMapper;
using Hng.Application.Features.Roles.Dto;
using Hng.Application.Features.Roles.Handler;
using Hng.Application.Features.Roles.Queries;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Hng.Graphql.Features.Roles
{
    public class RolesQueries
    {
        public async Task<IEnumerable<PermissionDto>> GetAllPermissions(
            [FromServices] IRepository<RolePermission> permissionsRepo,
            [FromServices] IMapper mapper) =>
            await new GetRolePermissionsQueryHandler(permissionsRepo, mapper)
                .Handle(new GetRolePermissionsQuery(), CancellationToken.None);

        public async Task<RoleDetailsResponseDto> GetRoleById(
            Guid roleId,
            Guid orgId,
            [FromServices] IRepository<Role> roleRepository,
            [FromServices] IMapper mapper) =>
            await new GetRoleByIdQueryHandler(roleRepository, mapper)
            .Handle(new GetRoleByIdQuery(roleId, orgId), CancellationToken.None);
    }
}
