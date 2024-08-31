using MediatR;

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
