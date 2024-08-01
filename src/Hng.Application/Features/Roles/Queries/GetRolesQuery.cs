using Hng.Application.Features.Roles.Dto;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hng.Application.Features.Roles.Queries
{
    public class GetRolesQuery : IRequest<IEnumerable<RoleDto>>
    {
        public Guid OrganizationId { get; set; }
    }
}
