using Hng.Application.Features.Organisations.Dtos;
using Hng.Application.Shared.Dtos;
using MediatR;

namespace Hng.Application.Features.Organisations.Queries
{
    public class GetAllUsersOrganizationsQuery : IRequest<SuccessResponseDto<List<OrganizationDto>>>
    {

    }
}
