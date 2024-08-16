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

namespace Hng.Application.Features.OrganisationInvite.Handlers;

public class GetUniqueOrganisationInviteLinkQueryHandler(IRequestValidator requestValidator, IRepository<Organization> orgRepository) : IRequestHandler<GetUniqueOrganizationLinkQuery, StatusCodeResponse>
{
    private readonly IRequestValidator requestValidator = requestValidator;
    private readonly IRepository<Organization> orgRepository = orgRepository;
    
    // private readonly FrontendUrl frontendUrl = frontendUrl;

    public async Task<StatusCodeResponse> Handle(GetUniqueOrganizationLinkQuery request, CancellationToken cancellationToken)
    {

        Guid orgId = Guid.Parse(request.Details.OrganizationId);
        Result<Organization> validationResult = await ValidateRequest(request, orgId);


        if (!validationResult.IsSuccess)
        {
            return ErrorResponse(validationResult);
        }
        Organization organization = validationResult.Value;

        // if (organization.InviteToken == Guid.Empty)
        // {
        //     organization.InviteToken = Guid.NewGuid();
        // }

        return new StatusCodeResponse()
        {
            Message = "Invite link fetched successfully",
            Data = organization,
            // {
            //     invite_link = $"{frontendUrl}invite?{organization.InviteToken}&org_id={organization.Id}"
            // },
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
