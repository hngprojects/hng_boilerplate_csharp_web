using MediatR;

namespace Hng.Application.Features.Organisations.Commands
{
    public class DeleteUserOrganizationCommand : IRequest<bool>
    {
        public DeleteUserOrganizationCommand(Guid organizationId, Guid userId)
        {
            OrganizationId = organizationId;
            UserId = userId;
        }

        public Guid OrganizationId { get; }
        public Guid UserId { get; }
    }
}
