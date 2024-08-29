using Hng.Application.Shared.Dtos;
using MediatR;

namespace Hng.Application.Features.Roles.Command
{
    public class AssignRoleCommand : IRequest<SuccessResponseDto<object>>
    {
        public AssignRoleCommand(Guid orgId, Guid roleId, Guid userId)
        {
            OrgId = orgId;
            RoleId = roleId;
            UserId = userId;
        }

        public Guid OrgId { get; }
        public Guid RoleId { get; }
        public Guid UserId { get; }
    }
}
