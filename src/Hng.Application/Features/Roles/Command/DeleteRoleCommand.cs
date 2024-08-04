using Hng.Application.Features.Roles.Dto;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hng.Application.Features.Roles.Command
{
    public class DeleteRoleCommand : IRequest<DeleteRoleResponseDto>
    {
        public DeleteRoleCommand(Guid organizationId, Guid roleId)
        {
            OrganizationId = organizationId;
            RoleId = roleId;
        }

        public Guid OrganizationId { get; }
        public Guid RoleId { get; }
    }

}
