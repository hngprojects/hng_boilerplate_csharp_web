using AutoMapper;
using Hng.Application.Features.Organisations.Dtos;
using Hng.Application.Features.Organisations.Queries;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using Hng.Infrastructure.Services.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Hng.Application.Features.Organisations.Handlers;

public class GetOrganizationByIdQueryHandler(IRepository<Organization> organizationRepository, IMapper mapper, IAuthenticationService authenticationService)
    : IRequestHandler<GetOrganizationByIdQuery, GetOrganisationResponseDto>
{
    private readonly IAuthenticationService _authenticationService = authenticationService;
    public async Task<GetOrganisationResponseDto> Handle(GetOrganizationByIdQuery request, CancellationToken cancellationToken)
    {
        var loggedInUserId = await _authenticationService.GetCurrentUserAsync();
        var organization = await organizationRepository.GetBySpec(
            o => o.Id == request.OrganizationId,
            o => o.Users);

        if (organization is null)
        {
            return new GetOrganisationResponseDto()
            {
                StatusCode = StatusCodes.Status404NotFound,
                Message = "Organisation Not Found",
            };
        }

        if (organization.OwnerId != loggedInUserId)
        {
            return new GetOrganisationResponseDto()
            {
                StatusCode = StatusCodes.Status400BadRequest,
                Message = "You are not authorised to view this organisation",
            };
        }

        var organisationDto = mapper.Map<OrganizationDto>(organization);

        return new GetOrganisationResponseDto()
        {
            StatusCode = StatusCodes.Status200OK,
            Message = "Organisation successfully retrieved",
            Data = organisationDto
        };

    }
}