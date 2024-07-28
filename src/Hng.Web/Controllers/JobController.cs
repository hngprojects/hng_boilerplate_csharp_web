using Hng.Application.Features.Jobs.Commands;
using Hng.Application.Features.Jobs.Dtos;
using Hng.Application.Features.Jobs.Queries;
using Hng.Application.Shared.Dtos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hng.Web.Controllers;

[Authorize]
[ApiController]
[Route("api/v1/jobs")]
public class JobController(IMediator mediator)
    : ControllerBase
{

    [HttpPost]
    [ProducesResponseType(typeof(JobDto), StatusCodes.Status201Created)]
    public async Task<ActionResult<JobDto>> CreateJob([FromBody] CreateJobDto body)
    {
        var command = new CreateJobCommand(body);
        var response = await mediator.Send(command);
        return CreatedAtAction(nameof(CreateJob), response);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(JobDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(FailureResponseDto<string>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<JobDto>> GetJobById(Guid id)
    {
        var query = new GetJobByIdQuery(id);
        var response = await mediator.Send(query);

        return response is null ? NotFound(new FailureResponseDto<string>
        {
            Data = null,
            Error = "Job not found",
            Message = "The requested job does not exist."
        }) : Ok(response);
    }
}