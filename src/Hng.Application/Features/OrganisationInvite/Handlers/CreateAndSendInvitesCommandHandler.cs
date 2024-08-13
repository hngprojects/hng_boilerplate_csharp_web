using Hng.Application.Features.OrganisationInvite.Commands;
using Hng.Application.Features.OrganisationInvite.Dtos;
using Hng.Application.Features.OrganisationInvite.Validators;
using Hng.Application.Features.OrganisationInvite.Validators.ValidationErrors;
using Hng.Application.Shared.Dtos;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using Hng.Infrastructure.Services.Interfaces;
using Hng.Infrastructure.Utilities;
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
    StatusCodeResponse<object>>
{
    private readonly IOrganisationInviteService inviteService = inviteService;
    private readonly IMessageQueueService queueService = queueService;
    private readonly IRepository<User> userRepository = userRepository;
    private readonly IRepository<Organization> organizationRepository = organizationRepository;
    private readonly IRepository<OrganizationInvite> inviteRepository = inviteRepository;
    private readonly IRequestValidator requestValidator = requestValidator;

    public async Task<StatusCodeResponse<object>> Handle(CreateAndSendInvitesCommand request, CancellationToken cancellationToken)
    {
        Guid orgId = Guid.Parse(request.Details.OrgId);

        Result<Organization> validationResult = await ValidateRequest(request, orgId, organizationRepository);

        if (!validationResult.IsSuccess) return ErrorResponse(validationResult);

        CreateAndSendInvitesDto details = request.Details;

        List<InviteDto> inviteList = [];
        User user = await userRepository.GetAsync(details.InviterId);
        string inviterName = user.FirstName;

        Organization organization = validationResult.Value;

        foreach (string email in details.Emails)
        {
            inviteList.Add(await GenerateInvite(email, details.InviterId, orgId, inviteRepository));
        }
        throw new NotImplementedException();
    }

    private async Task<InviteDto> GenerateInvite(string email, Guid inviterId, Guid orgId, IRepository<OrganizationInvite> inviteRepository)
    {
        Result<OrganizationInvite> uniqueInviteCheck = await requestValidator.InviteDoesNotExistAsync(orgId, email, inviteRepository);

        OrganizationInvite invite = null;
        if (uniqueInviteCheck.IsSuccess) invite = await inviteService.CreateInvite(inviterId, orgId, email);

        InviteDto inviteDto = new() { Email = email };
        if (invite is null)
        {
            inviteDto.Error = InviteAlreadyExistsError.FromEmail(email).Message;
            return inviteDto;
        }
        inviteDto.InviteLink = invite.InviteLink.ToString();
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

    private static StatusCodeResponse<object> ErrorResponse(Result<Organization> errorResult)
    {
        return new StatusCodeResponse<object>()
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



