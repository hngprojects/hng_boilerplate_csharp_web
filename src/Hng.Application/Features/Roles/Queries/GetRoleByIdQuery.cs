using Hng.Application.Features.Roles.Dto;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hng.Application.Features.Roles.Queries
{
    public class GetRoleByIdQuery : IRequest<RoleDetailsDto>
    {
        public Guid OrganizationId { get; set; }
        public Guid RoleId { get; set; }
    }
}
