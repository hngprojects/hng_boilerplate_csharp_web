using System.Security.Claims;
using Hng.Application.Features.OrganisationInvite.Commands;
using Hng.Application.Features.OrganisationInvite.Dtos;
using Hng.Application.Features.Organisations.Commands;
using Hng.Application.Features.Organisations.Dtos;
using Hng.Application.Features.Organisations.Queries;
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


    [HttpPost("invite")]
    [ProducesResponseType(typeof(CreateOrganizationDto), StatusCodes.Status201Created)]

    public async Task<ActionResult<CreateOrganizationDto>> CreateOrganizationInvite([FromBody] CreateOrganizationInviteDto body)
    {
        var inviterIdString = HttpContext.User.FindFirst(ClaimTypes.Sid)!.Value;
        var inviterId = Guid.Parse(inviterIdString);
        body.UserId = inviterId;
        var command = new CreateOrganizationInviteCommand(body);
        var response = await mediator.Send(command);

        if (response == null)
        {
            return UnprocessableEntity();
        }

        return this.CreatedResult("Invitation created successfully", new { invitation = response });
    }
}