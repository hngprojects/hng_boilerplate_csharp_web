using Hng.Application.Features.Roles.Dto;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hng.Application.Features.Roles.Command
{
    public class UpdateRoleCommand : IRequest<UpdateRoleResponseDto>
    {
        public UpdateRoleCommand(Guid orgId,Guid roleId,UpdateRoleRequestDto uptRoleRequest)
        {
            OrgId = orgId;
            RoleId = roleId;
            UPTRoleRequest = uptRoleRequest;
        }

        public Guid OrgId { get; }
        public Guid RoleId { get; }
        public UpdateRoleRequestDto UPTRoleRequest { get; }
    }

}
