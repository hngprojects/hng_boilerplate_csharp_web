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
        public UpdateRoleCommand(UpdateRoleRequestDto uptRoleRequest)
        {
            UPTRoleRequest = uptRoleRequest;
        }

        public UpdateRoleRequestDto UPTRoleRequest { get; }
    }

}
