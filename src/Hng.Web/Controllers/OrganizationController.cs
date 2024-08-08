using Hng.Application.Features.Organisations.Commands;
using Hng.Application.Features.Organisations.Dtos;
using Hng.Application.Features.Organisations.Queries;
using Hng.Application.Features.Roles.Command;
using Hng.Application.Features.Roles.Dto;
using Hng.Application.Features.Roles.Queries;
using Hng.Application.Shared.Dtos;
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

    [HttpGet]
    [ProducesResponseType(typeof(SuccessResponseDto<List<OrganizationDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<SuccessResponseDto<List<OrganizationDto>>>> GetOrganizations()
    {
        var query = new GetAllUsersOrganizationsQuery();
        var response = await mediator.Send(query);
        return Ok(response);
    }

    [HttpPost]
    [ProducesResponseType(typeof(OrganizationDto), StatusCodes.Status201Created)]
    public async Task<ActionResult<OrganizationDto>> CreateOrganization([FromBody] CreateOrganizationDto body)
    {
        var command = new CreateOrganizationCommand(body);
        var response = await mediator.Send(command);
        return CreatedAtAction(nameof(CreateOrganization), response);
    }

    /// <summary>
    /// Create Role For Organization
    /// </summary>
    [HttpPost("{orgId:guid}/roles")]
    [ProducesResponseType(typeof(CreateRoleResponseDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateRole(Guid orgId, [FromBody] CreateRoleRequestDto request)
    {
        CreateRoleCommand command = new CreateRoleCommand(orgId, request);
        var response = await mediator.Send(command);
        return StatusCode(response.StatusCode, response);
    }

    /// <summary>
    /// Get All Roles In Organisation
    /// </summary>
    [HttpGet("{orgId:guid}/roles")]
    [ProducesResponseType(typeof(IEnumerable<RoleDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetRoles(Guid orgId)
    {
        var query = new GetRolesQuery(orgId);
        var roles = await mediator.Send(query);
        return Ok(new { status_code = 200, data = roles });
    }

    /// <summary>
    /// Get Organizations Role By Id
    /// </summary>
    [HttpGet("{orgId:guid}/roles/{roleId}")]
    [ProducesResponseType(typeof(RoleDetailsDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetRole(Guid orgId, Guid roleId)
    {
        var query = new GetRoleByIdQuery(orgId, roleId);
        var roleDetails = await mediator.Send(query);
        return StatusCode(roleDetails.StatusCode, new { roleDetails.StatusCode, roleDetails.Id, roleDetails.Name, roleDetails.Description, roleDetails.Permissions });
    }

    /// <summary>
    /// Update Organisations Role
    /// </summary>
    [HttpPut("{orgId:guid}/roles/{roleId}")]
    [ProducesResponseType(typeof(UpdateRoleResponseDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateRole(Guid orgId, Guid roleId, [FromBody] UpdateRoleRequestDto request)
    {
        UpdateRoleCommand command = new(orgId, roleId, request);
        var response = await mediator.Send(command);
        return StatusCode(response.StatusCode, response);
    }

    /// <summary>
    /// Delete Organizations Role
    /// </summary>
    [HttpDelete("{orgId:guid}/roles/{roleId}")]
    [ProducesResponseType(typeof(DeleteRoleResponseDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> DeleteRole(Guid orgId, Guid roleId)
    {
        var command = new DeleteRoleCommand(orgId, roleId);
        var response = await mediator.Send(command);
        return StatusCode(response.StatusCode, response);
    }
}