using Hng.Application.Features.Organisations.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hng.Application.Features.Organisations.Commands
{
    public class DeleteUserOrganizationCommand : IRequest<bool>
    {
        public DeleteUserOrganizationCommand(Guid organizationId)
        {
            OrganizationId = organizationId;
        }

        public Guid OrganizationId { get; }
    }
}
