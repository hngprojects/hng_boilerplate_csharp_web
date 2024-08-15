using AutoMapper;
using Hng.Application.Features.Organisations.Commands;
using Hng.Application.Features.Organisations.Dtos;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using Hng.Infrastructure.Services.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Hng.Application.Features.Organisations.Handlers;

public class CreateOrganisationCommandHandler(IRepository<Organization> organizationRepository, IMapper mapper, IAuthenticationService authenticationService)
    : IRequestHandler<CreateOrganizationCommand, CreateOrganisationResponseDto>
{
    private readonly IRepository<Organization> _organizationRepository = organizationRepository;
    private readonly IMapper _mapper = mapper;
    private readonly IAuthenticationService _authenticationService = authenticationService;


    public async Task<CreateOrganisationResponseDto> Handle(CreateOrganizationCommand request, CancellationToken cancellationToken)
    {
        var loggedInUserId = await _authenticationService.GetCurrentUserAsync();
        var organisation = _mapper.Map<Organization>(request.OrganizationBody);

        organisation.CreatedAt = DateTime.UtcNow;
        organisation.UpdatedAt = DateTime.UtcNow;
        organisation.OwnerId = loggedInUserId;

        await _organizationRepository.AddAsync(organisation);
        await _organizationRepository.SaveChanges();
        
        var organisationDto = _mapper.Map<OrganizationDto>(organisation);
        
        return new CreateOrganisationResponseDto
        {
            StatusCode = StatusCodes.Status201Created,
            Message = "Organisation Created Successfully",
            Data = organisationDto
        };
    }
}