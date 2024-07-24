using Hng.Application.Features.Organisations.Dtos;
using MediatR;

namespace Hng.Application.Features.Organisations.Queries;

public class GetOrganizationByIdQuery(Guid id) : IRequest<OrganizationDto>
{
    public Guid OrganizationId { get; } = id;
}