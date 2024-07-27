using Hng.Application.Features.Jobs.Commands;
using Hng.Application.Features.Jobs.Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Hng.Web.Controllers;

[ApiController]
[Route("api/v1/jobs")]
public class JobController(IMediator mediator) : ControllerBase
{

    [HttpPost]
    [ProducesResponseType(typeof(JobDto), StatusCodes.Status201Created)]
    public async Task<ActionResult<JobDto>> CreateJob([FromBody] CreateJobDto body)
    {
        var command = new CreateJobCommand(body);
        var response = await mediator.Send(command);
        return CreatedAtAction(nameof(CreateJob), response);
    }
}