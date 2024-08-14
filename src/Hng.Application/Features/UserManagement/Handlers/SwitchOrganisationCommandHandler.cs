using Hng.Application.Features.Organisations.Commands;
using Hng.Application.Features.Organisations.Dtos;
using Hng.Application.Features.UserManagement.Commands;
using Hng.Application.Features.UserManagement.Dtos;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using Hng.Infrastructure.Services.Interfaces;
using MediatR;

namespace Hng.Application.Features.UserManagement.Handlers;

public class SwitchOrganisationCommandHandler(IRepository<User> userRepository, IRepository<Organization> organisationRepository, IAuthenticationService authenticationService) : IRequestHandler<SwitchOrganisationCommand, SwitchOrganisationResponseDto>
{
    private readonly IRepository<User> _userRepository = userRepository;
    private readonly IRepository<Organization> _organisationRepository = organisationRepository;
    private readonly IAuthenticationService _authenticationService = authenticationService;

    public async Task<SwitchOrganisationResponseDto> Handle(SwitchOrganisationCommand request, CancellationToken cancellationToken)
    {
        var loggedInUserId = await _authenticationService.GetCurrentUserAsync();
        var organisation = await _organisationRepository.GetBySpec(
            o => o.Id == request.OrganisationId);
        
        if (organisation is null)
        {
            return new SwitchOrganisationResponseDto
            {
                Message = "Organization not found."
            };
        }

        var isMember = organisation.Users.Any(user => user.Id == loggedInUserId);
        if (!isMember)
        {
            return new SwitchOrganisationResponseDto
            {
                Message = "Unauthorized request. You are not a member of this organisation."
            };
        }

        var user = await _userRepository.GetBySpec(u => u.Id == loggedInUserId);

        var currentActiveOrganisation = user.Organizations.FirstOrDefault(o => o.IsActive);
        if (currentActiveOrganisation != null && currentActiveOrganisation.Id == request.OrganisationId && request.IsActive)
        {
            return new SwitchOrganisationResponseDto
            {
                Message = "No change required. The organization is already active."
            };
        }

        foreach (var org in user.Organizations)
            org.IsActive = org.Id == request.OrganisationId && request.IsActive;

        await _userRepository.UpdateAsync(user);
        await _userRepository.SaveChanges();

        return new SwitchOrganisationResponseDto
        {
            Message = "Organisation successfully updated",
            OrganisationDto = new OrganizationDto { Id = request.OrganisationId, IsActive = request.IsActive }
        };
    }
}