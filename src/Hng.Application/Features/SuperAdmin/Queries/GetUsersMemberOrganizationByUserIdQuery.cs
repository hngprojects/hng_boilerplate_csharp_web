using Hng.Application.Features.SuperAdmin.Dto;
using Hng.Application.Shared.Dtos;
using MediatR;

namespace Hng.Application.Features.SuperAdmin.Queries
{
    public class GetUsersMemberOrganizationsByUserIdQuery : IRequest<PagedListDto<OrganizationDto>>
    {
        public GetUsersMemberOrganizationsByUserIdQuery(Guid userId, BaseQueryParameters userMemberOrganizationsQueryParameter)
        {
            UserId = userId;
            UserMemberOrganizationsQueryParameter = userMemberOrganizationsQueryParameter;
        }

        public Guid UserId { get; set; }
        public BaseQueryParameters UserMemberOrganizationsQueryParameter { get; set; }
    }
}
