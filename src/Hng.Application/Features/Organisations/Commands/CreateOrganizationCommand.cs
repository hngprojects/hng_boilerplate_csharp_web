using Hng.Application.Features.Organisations.Dtos;
using MediatR;

namespace Hng.Application.Features.Organisations.Commands;

public class CreateOrganizationCommand : IRequest<OrganizationDto>, IRequest<CreateOrganisationResponseDto>
{
    public CreateOrganizationCommand(CreateOrganizationDto createOrganizationDto)
    {
        OrganizationBody = createOrganizationDto;
    }

    public CreateOrganizationDto OrganizationBody { get; }

}