using Hng.Application.Features.Organisations.Commands;
using Hng.Application.Features.Organisations.Dtos;
using Hng.Application.Features.Organisations.Queries;
using Hng.Application.Features.Roles.Command;
using Hng.Application.Features.Roles.Queries;
using Hng.Domain.Entities;
using Hng.Infrastructure.Services;
using Hng.Infrastructure.Services.Interfaces;
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
    [HttpPost("organizations/{orgId}/roles")]
    public async Task<IActionResult> CreateRole(Guid orgId, [FromBody] CreateRoleCommand command)
    {
        command.OrganizationId = orgId;
        var response = await mediator.Send(command);
        return StatusCode(response.StatusCode, response);
    }
    [HttpGet("organizations/{orgId}/roles")]
    public async Task<IActionResult> GetRoles(Guid orgId)
    {
        var query = new GetRolesQuery { OrganizationId = orgId };
        var roles = await mediator.Send(query);
        return Ok(new { status_code = 200, data = roles });
    }

    [HttpGet("organizations/{orgId}/roles/{roleId}")]
    public async Task<IActionResult> GetRole(Guid orgId, Guid roleId)
    {
        var query = new GetRoleByIdQuery { OrganizationId = orgId, RoleId = roleId };
        var roleDetails = await mediator.Send(query);
        return StatusCode(roleDetails.StatusCode, new { roleDetails.StatusCode, roleDetails.Id, roleDetails.Name, roleDetails.Description, roleDetails.Permissions });
    }



}