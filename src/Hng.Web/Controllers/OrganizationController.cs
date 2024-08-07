﻿using System.Net;
using System.Security.Claims;
using Hng.Application.Features.OrganisationInvite.Commands;
using Hng.Application.Features.OrganisationInvite.Dtos;
using Hng.Application.Features.Organisations.Commands;
using Hng.Application.Features.Organisations.Dtos;
using Hng.Application.Features.Organisations.Queries;
using Hng.Application.Features.Roles.Command;
using Hng.Application.Features.Roles.Dto;
using Hng.Application.Features.Roles.Queries;
using Hng.Application.Shared.Dtos;
using Hng.Domain.Common;
using Hng.Infrastructure.Services.Interfaces;
using Hng.Infrastructure.Utilities.Errors.OrganisationInvite;
using Hng.Web.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hng.Web.Controllers;

[Authorize]
[ApiController]
[Route("api/v1/organizations")]
public class OrganizationController(IMediator mediator, IAuthenticationService authenticationService) : ControllerBase
{
    private readonly IAuthenticationService authenticationService = authenticationService;

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

    [HttpPost("{id}/invite")]
    [ProducesResponseType(typeof(CreateOrganizationDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(FailureResponseDto<string>), (int)HttpStatusCode.Conflict)]
    [ProducesResponseType(typeof(FailureResponseDto<string>), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(FailureResponseDto<string>), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(FailureResponseDto<string>), (int)HttpStatusCode.UnprocessableContent)]

    public async Task<ActionResult<CreateOrganizationDto>> CreateOrganizationInvite([FromBody] CreateOrganizationInviteDto body, string id)
    {
        var inviterId = await authenticationService.GetCurrentUserAsync();
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