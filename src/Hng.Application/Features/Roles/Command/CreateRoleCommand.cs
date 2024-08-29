using Hng.Application.Features.Roles.Dto;
using MediatR;

namespace Hng.Application.Features.Roles.Command
{
    public class CreateRoleCommand : IRequest<CreateRoleResponseDto>
    {
        public CreateRoleCommand(Guid organizationId, CreateRoleRequestDto roleRequest)
        {
            OrganizationId = organizationId;
            RoleRequestBody = roleRequest;
        }

        public Guid OrganizationId { get; }
        public CreateRoleRequestDto RoleRequestBody { get; }
    }
}
