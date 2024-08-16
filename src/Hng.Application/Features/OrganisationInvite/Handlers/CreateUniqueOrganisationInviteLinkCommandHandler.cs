using Hng.Application.Features.OrganisationInvite.Commands;
using Hng.Application.Features.OrganisationInvite.Validators;
using Hng.Application.Shared.Dtos;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using Hng.Infrastructure.Utilities.Results;
using MediatR;

namespace Hng.Application.Features.OrganisationInvite.Handlers;

public class CreateUniqueOrganisationInviteLinkCommandHandler(IRequestValidator requestValidator, IRepository<Organization> orgRepository) : IRequestHandler<CreateUniqueOrganisationInviteLinkCommand, StatusCodeResponse>
{
    private readonly IRequestValidator requestValidator = requestValidator;
    private readonly IRepository<Organization> orgRepository = orgRepository;

    public async Task<StatusCodeResponse> Handle(CreateUniqueOrganisationInviteLinkCommand request, CancellationToken cancellationToken)
    {
        Guid orgId = Guid.Parse(request.Details.OrganizationId);
        Result<Organization> validateResult = await requestValidator.UserIsOrganizationOwnerAsync(request.Details.UserId, orgId, orgRepository);
        var response = new StatusCodeResponse() { Message = "request processed" };
        if (!validateResult.IsSuccess)
        {
            response.Message = validateResult.Error.Message;
            response.StatusCode = 422;
            return response;
        }
        response.Data = validateResult.Value;
        return response;

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
}
