using System.Text.Json;
using Hng.Application.Features.OrganisationInvite.Queries;
using Hng.Application.Features.OrganisationInvite.Validators;
using Hng.Application.Features.OrganisationInvite.Validators.ValidationErrors;
using Hng.Application.Shared.Dtos;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using Hng.Infrastructure.Utilities;
using Hng.Infrastructure.Utilities.Results;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Hng.Application.Features.OrganisationInvite.Handlers;

public class GetUniqueOrganisationInviteLinkQueryHandler(
    IOptions<FrontendUrl> frontendUrl,
    IRequestValidator requestValidator,
    IRepository<Organization> orgRepository,
    ILogger<GetUniqueOrganisationInviteLinkQueryHandler> logger) : IRequestHandler<GetUniqueOrganizationLinkQuery, StatusCodeResponse>
{
    private readonly IRequestValidator requestValidator = requestValidator;
    private readonly IRepository<Organization> orgRepository = orgRepository;

    private readonly ILogger<GetUniqueOrganisationInviteLinkQueryHandler> logger = logger;

    private readonly FrontendUrl frontendUrl = frontendUrl.Value;

    public async Task<StatusCodeResponse> Handle(GetUniqueOrganizationLinkQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Called with {query}", JsonSerializer.Serialize(request));

        Guid orgId = Guid.Parse(request.Details.OrganizationId);
        Result<Organization> validationResult = await ValidateRequest(request, orgId);

        if (!validationResult.IsSuccess)
        {
            return ErrorResponse(validationResult);
        }
        Organization organization = validationResult.Value;

        if (organization.InviteToken == Guid.Empty)
        {
            logger.LogInformation("Generating an invite token for the organization {organization}", JsonSerializer.Serialize(organization));
            organization.InviteToken = Guid.NewGuid();
        }

        await orgRepository.SaveChanges();
        return new StatusCodeResponse()
        {
            Message = "Invite link fetched successfully",
            Data = new
            {
                invite_link = $"{frontendUrl.Path}invite?{organization.InviteToken}&org_id={organization.Id}"
            },
            StatusCode = StatusCodes.Status200OK
        };

    }

    private async Task<Result<Organization>> ValidateRequest(GetUniqueOrganizationLinkQuery request, Guid orgId)
    {
        Result<Organization> validationResult = await requestValidator.UserIsOrganizationOwnerAsync(request.Details.UserId, orgId, orgRepository);
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
