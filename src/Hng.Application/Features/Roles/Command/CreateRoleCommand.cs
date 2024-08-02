using Hng.Application.Features.Roles.Dto;
using Hng.Application.Features.UserManagement.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hng.Application.Features.Roles.Command
{
    public class CreateRoleCommand : IRequest<CreateRoleResponseDto>
    {
        public CreateRoleCommand(CreateRoleRequestDto roleRequest)
        {
            RoleRequestBody = roleRequest;
        }

        public CreateRoleRequestDto RoleRequestBody { get; }
    }
}
