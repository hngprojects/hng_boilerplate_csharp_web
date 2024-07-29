using Hng.Application.Features.Organisations.Commands;
using Hng.Application.Features.Organisations.Dtos;
using Hng.Application.Features.Organisations.Queries;
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
public class OrganizationController(IMediator mediator, IMessageQueueService messageQueueService) : ControllerBase
{
    private readonly IMessageQueueService messageQueueService = messageQueueService;

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
    [AllowAnonymous]
    [ProducesResponseType(typeof(OrganizationDto), StatusCodes.Status201Created)]
    public async Task<ActionResult<OrganizationDto>> CreateOrganization([FromBody] CreateOrganizationDto body)
    {
        await messageQueueService.TryQueueEmail(new Message()
        {
            Type = Domain.Enums.MessageType.Email,
            Recipient = "user@example.com",
            Content = "This is a test email to demonstrate some stuff",

        });
        return Ok();
        // var command = new CreateOrganizationCommand(body);
        // var response = await mediator.Send(command);
        // return CreatedAtAction(nameof(CreateOrganization), response);
    }
}