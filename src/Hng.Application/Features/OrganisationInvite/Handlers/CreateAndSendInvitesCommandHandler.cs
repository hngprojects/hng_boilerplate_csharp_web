using Hng.Application.Features.OrganisationInvite.Commands;
using Hng.Application.Features.OrganisationInvite.Dtos;
using Hng.Application.Features.OrganisationInvite.Validators;
using Hng.Application.Features.OrganisationInvite.Validators.ValidationErrors;
using Hng.Application.Shared.Dtos;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using Hng.Infrastructure.Services.Interfaces;
using Hng.Infrastructure.Utilities.Results;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Hng.Application.Features.OrganisationInvite.Handlers;

public class CreateAndSendInvitesCommandHandler(
    IOrganisationInviteService inviteService,
    IMessageQueueService queueService,
    IRepository<User> userRepository,
    IRepository<Organization> organizationRepository,
    IRepository<OrganizationInvite> inviteRepository
    , IRequestValidator requestValidator) :

    IRequestHandler<CreateAndSendInvitesCommand,
    StatusCodeResponse>
{
    private readonly IOrganisationInviteService inviteService = inviteService;
    private readonly IMessageQueueService queueService = queueService;
    private readonly IRepository<User> userRepository = userRepository;
    private readonly IRepository<Organization> organizationRepository = organizationRepository;
    private readonly IRepository<OrganizationInvite> inviteRepository = inviteRepository;
    private readonly IRequestValidator requestValidator = requestValidator;

    public async Task<StatusCodeResponse> Handle(CreateAndSendInvitesCommand request, CancellationToken cancellationToken)
    {
        Guid orgId = Guid.Parse(request.Details.OrgId);

        Result<Organization> validationResult = await ValidateRequest(request, orgId, organizationRepository);

        if (!validationResult.IsSuccess) return ErrorResponse(validationResult);

        CreateAndSendInvitesDto details = request.Details;

        List<InviteDto> inviteList = [];
        User user = await userRepository.GetAsync(details.InviterId);

        Organization organization = validationResult.Value;

        foreach (string email in details.Emails)
        {
            inviteList.Add(await CreateAndSendInvites(user, organization, email));
        }

        return new StatusCodeResponse
        {
            StatusCode = StatusCodes.Status200OK,
            Message = "Invitation(s) processed successfully!",
            Data = new CreateAndSendInvitesResponseDto() { Invitations = inviteList }
        };
    }

    private async Task<InviteDto> CreateAndSendInvites(User inviter, Organization org, string inviteeEmail)
    {
        Result<OrganizationInvite> uniqueInviteCheck = await requestValidator.InviteDoesNotExistAsync(org.Id, inviteeEmail, inviteRepository);

        InviteDto inviteDto = new() { Email = inviteeEmail };

        if (!uniqueInviteCheck.IsSuccess)
        {
            inviteDto.Error = InviteAlreadyExistsError.FromEmail(inviteeEmail).Message;
            return inviteDto;
        }

        OrganizationInvite invite = await inviteService.CreateInvite(inviter.Id, org.Id, inviteeEmail);
        string inviteLink = inviteService.GenerateInviteUrlFromToken(invite.InviteCode);
        await queueService.SendInviteEmailAsync(inviter.FirstName, inviteeEmail, org.Name, invite.ExpiresAt, inviteLink);
        await inviteRepository.SaveChanges();

        inviteDto.InviteLink = inviteLink;
        return inviteDto;

    }

    private async Task<Result<Organization>> ValidateRequest(
        CreateAndSendInvitesCommand request,
        Guid orgId,
        IRepository<Organization> organizationRepository)
    {
        Result<Organization> validationResult = await requestValidator.UserIsOrganizationOwnerAsync(
            request.Details.InviterId,
            orgId, organizationRepository);
        return validationResult;
    }

    private static StatusCodeResponse ErrorResponse(Result<Organization> errorResult)
    {
        return new StatusCodeResponse()
        {
            Message = errorResult.Error.Message,
            StatusCode = errorResult.Error switch
            {
                OrganisationDoesNotExistError => StatusCodes.Status404NotFound,
                UserIsNotOwnerError => StatusCodes.Status401Unauthorized,
                _ => StatusCodes.Status422UnprocessableEntity
            }
        };



    }
}



