using Hng.Application.Features.UserManagement.Commands;
using Hng.Application.Features.UserManagement.Dtos;
using Hng.Application.Features.UserManagement.Queries;
using Hng.Application.Shared.Dtos;
using Hng.Application.Shared;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace Hng.Web.Controllers;

[Authorize]
[ApiController]
[Route("api/v1/users")]
public class UserController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<UserDto>> GetUserById(Guid id)
    {
        var query = new GetUserByIdQuery(id);
        var response = await _mediator.Send(query);
        return response is null
            ? NotFound(new
            {
                message = "User not found",
                is_successful = false,
                status_code = 404
            })
            : Ok(response);
    }

    [HttpGet("")]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
    {
        var users = await _mediator.Send(new GetUsersQuery());
        return Ok(users);
    }

    [HttpGet("export/csv")]
    //[Authorize(Roles = "SuperAdmin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(FailureResponseDto<object>), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(FailureResponseDto<object>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ExportUsersToCsv()
    {
        try
        {
            var csvBytes = await _mediator.Send(new ExportUsersToCsvQuery());

            var fileName = $"users_export_{DateTime.UtcNow:yyyy-MM-dd}.csv";
            return File(csvBytes, MediaTypeNames.Application.Octet, fileName);
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized(new FailureResponseDto<object>
            {
                StatusCode = 401,
                Message = "Unauthorized access. You must have Super Admin privileges to export user data.",

            });
        }
    }

    [HttpPut("organisations/{organisationId:guid}")]
    public async Task<IActionResult> SwitchUserOrganisation(
        Guid organisationId,
        [FromBody] SwitchOrganisationRequestDto request)
    {
        var command = new SwitchOrganisationCommand
        {
            OrganisationId = organisationId,
            IsActive = request.IsActive
        };

        var response = await _mediator.Send(command);
        return Ok(response);

    }
}