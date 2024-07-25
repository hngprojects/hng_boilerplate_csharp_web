using AutoMapper;
using Hng.Application.Features.Organisations.Dtos;
using Hng.Application.Features.Organisations.Queries;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using MediatR;

namespace Hng.Application.Features.Organisations.Handlers;

public class GetOrganizationByIdQueryHandler(IRepository<Organization> organizationRepository, IMapper mapper)
    : IRequestHandler<GetOrganizationByIdQuery, OrganizationDto>
{
    public async Task<OrganizationDto> Handle(GetOrganizationByIdQuery request, CancellationToken cancellationToken)
    {
        var organization = await organizationRepository.GetBySpec(
            o => o.Id == request.OrganizationId,
            o => o.Users);

        return organization == null ? null : mapper.Map<OrganizationDto>(organization);
    }
}