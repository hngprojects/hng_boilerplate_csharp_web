using Hng.Application.Features.SuperAdmin.Dto;
using Hng.Application.Shared.Dtos;
using MediatR;

namespace Hng.Application.Features.SuperAdmin.Queries;

public class GetUsersOwnedOrganizationsByUserIdQuery : IRequest<PagedListDto<OrganizationDto>>
{
    public GetUsersOwnedOrganizationsByUserIdQuery(Guid userId, BaseQueryParameters userOwnedOrganizationsQueryParameter)
    {
        UserId = userId;
        UserOwnedOrganizationsQueryParameter = userOwnedOrganizationsQueryParameter;
    }
    public Guid UserId { get; set; }
    public BaseQueryParameters UserOwnedOrganizationsQueryParameter { get; set; }
}