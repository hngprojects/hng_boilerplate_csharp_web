using System.Net;
using System.Security.Claims;
using Hng.Application.Features.OrganisationInvite.Commands;
using Hng.Application.Features.OrganisationInvite.Dtos;
using Hng.Application.Features.Organisations.Commands;
using Hng.Application.Features.Organisations.Dtos;
using Hng.Application.Features.Organisations.Queries;
using Hng.Application.Shared.Dtos;
using Hng.Domain.Common;
using Hng.Infrastructure.Utilities.Errors.OrganisationInvite;
using Hng.Web.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hng.Web.Controllers;

[Authorize]
[ApiController]
[Route("api/v1/organizations")]
public class OrganizationController(IMediator mediator) : ControllerBase
{

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(OrganizationDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<OrganizationDto>> GetOrganizationById(Guid id)
    {
        var query = new GetOrganizationByIdQuery(id);
        var response = await mediator.Send(query);
        return response is null ? NotFound(new
        {
            message = "Organization not found",
            is_successful = false,
            status_code = 404
        }) : Ok(response);
    }

    [HttpPost]
    [ProducesResponseType(typeof(OrganizationDto), StatusCodes.Status201Created)]
    public async Task<ActionResult<OrganizationDto>> CreateOrganization([FromBody] CreateOrganizationDto body)
    {
        var command = new CreateOrganizationCommand(body);
        var response = await mediator.Send(command);
        return CreatedAtAction(nameof(CreateOrganization), response);
    }

    [HttpPost("{id}/invite")]
    [ProducesResponseType(typeof(CreateOrganizationDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(FailureResponseDto<string>), (int)HttpStatusCode.Conflict)]
    [ProducesResponseType(typeof(FailureResponseDto<string>), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(FailureResponseDto<string>), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(FailureResponseDto<string>), (int)HttpStatusCode.UnprocessableContent)]

    public async Task<ActionResult<CreateOrganizationDto>> CreateOrganizationInvite([FromBody] CreateOrganizationInviteDto body, string id)
    {
        var inviterIdString = HttpContext.User.FindFirst(ClaimTypes.Sid).Value!;
        var inviterId = Guid.Parse(inviterIdString);
        body.UserId = inviterId;
        body.OrganizationId = id;
        var command = new CreateOrganizationInviteCommand(body);
        Result<OrganizationInviteDto> result = await mediator.Send(command);

        if (result.IsSuccess) return this.CustomCreatedResult("Invitation created successfully", result.Value);
        FailureResponseDto<string> failureResponse = new()
        {
            Data = string.Empty,
            Error = "An error occured with your request",
            Message = result.Error.Message
        };

        return result.Error switch
        {
            InviteAlreadyExistsError => Conflict(failureResponse),
            OrganisationDoesNotExistError => NotFound(failureResponse),
            UserIsNotOwnerError => Unauthorized(failureResponse),
            (_) => UnprocessableEntity(failureResponse)
        };
    }
}