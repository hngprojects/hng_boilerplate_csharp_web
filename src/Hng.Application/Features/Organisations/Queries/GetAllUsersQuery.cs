using Hng.Application.Features.Organisations.Dtos;
using MediatR;

namespace Hng.Application.Features.Organisations.Queries
{
    public class GetAllUsersQuery : IRequest<OrganizationUserDto>
    {
        public GetAllUsersQuery(Guid organizationId)
        {
            OrganizationId = organizationId;
        }

        public Guid OrganizationId { get; }
    }
}
