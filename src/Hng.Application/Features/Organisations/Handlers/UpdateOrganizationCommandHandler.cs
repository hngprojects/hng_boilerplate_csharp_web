using AutoMapper;
using Hng.Application.Features.Organisations.Commands;
using Hng.Application.Features.Organisations.Dtos;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using Hng.Infrastructure.Services.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Hng.Application.Features.Organisations.Handlers
{
    public class UpdateOrganizationCommandHandler : IRequestHandler<UpdateOrganizationCommand, UpdateOrganizationResponseDto>
    {
        private readonly IRepository<Organization> _organizationRepository;
        private readonly IMapper _mapper;
        private readonly IAuthenticationService _authenticationService;

        public UpdateOrganizationCommandHandler(
            IRepository<Organization> organizationRepository,
            IMapper mapper,
            IAuthenticationService authenticationService)
        {
            _organizationRepository = organizationRepository;
            _mapper = mapper;
            _authenticationService = authenticationService;
        }

        public async Task<UpdateOrganizationResponseDto> Handle(UpdateOrganizationCommand request, CancellationToken cancellationToken)
        {
            var loggedInUserId = await _authenticationService.GetCurrentUserAsync();

            // Fetch the organization by ID
            var organization = await _organizationRepository.GetBySpec(x => x.Id == request.OrgId);

            if (organization is null)
            {
                return new UpdateOrganizationResponseDto
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = "Organization not found"
                };
            }

            // Ensure only the owner can update
            if (organization.OwnerId != loggedInUserId)
            {
                return new UpdateOrganizationResponseDto
                {
                    StatusCode = StatusCodes.Status403Forbidden,
                    Message = "You are not authorized to update this organization"
                };
            }

            // Update organization fields
            _mapper.Map(request.OrganizationBody, organization);
            organization.UpdatedAt = DateTime.UtcNow;

            // Save changes
            await _organizationRepository.UpdateAsync(organization);
            await _organizationRepository.SaveChanges();

            var updatedOrganizationDto = _mapper.Map<OrganizationDto>(organization);

            return new UpdateOrganizationResponseDto
            {
                StatusCode = StatusCodes.Status200OK,
                Message = "Organization updated successfully",
                Data = updatedOrganizationDto
            };
        }
    }

}
