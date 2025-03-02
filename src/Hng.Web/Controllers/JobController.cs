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
public class JobController : ControllerBase
{
    private readonly IMediator _mediator;

    public JobController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [ProducesResponseType(typeof(JobDto), StatusCodes.Status201Created)]
    public async Task<ActionResult<JobDto>> CreateJob([FromBody] CreateJobDto body)
    {
        var command = new CreateJobCommand(body);
        var response = await _mediator.Send(command);
        return CreatedAtAction(nameof(CreateJob), response);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(JobDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(FailureResponseDto<string>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<JobDto>> GetJobById(Guid id)
    {
        var query = new GetJobByIdQuery(id);
        var response = await _mediator.Send(query);

        return response is null ? NotFound(new FailureResponseDto<JobDto>
        {
            Data = null,
            Error = "Job not found",
            Message = "The requested job does not exist."
        }) : Ok(response);
    }

    [HttpGet("")]
    [ProducesResponseType(typeof(JobDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<JobDto>>> GetJobs()
    {
        var jobs = await _mediator.Send(new GetJobsQuery());
        return Ok(jobs);
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(FailureResponseDto<string>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult> DeleteJobById(Guid id)
    {
        await _mediator.Send(new DeleteJobByIdCommand(id));
        return NoContent();
    }


    /// <summary>
    /// Updates a job by ID
    /// </summary>
    /// <param name="request">The details of the job to upcate.</param>
    /// <returns>A response with the update result or an error message.</returns>
    [HttpPatch("{id:guid}")]
    [ProducesResponseType(typeof(UpdateJobResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(UpdateJobResponseDto), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(UpdateJobResponseDto), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<UpdateJobResponseDto>> UpdateJob(Guid id, [FromBody] UpdateJobDto updateRequest)
    {
        if (string.IsNullOrEmpty(updateRequest.Description) &&
            string.IsNullOrEmpty(updateRequest.Level.ToString()) &&
            string.IsNullOrEmpty(updateRequest.Company) &&
            string.IsNullOrEmpty(updateRequest.Title) &&
            string.IsNullOrEmpty(updateRequest.Location) &&
            updateRequest.Salary <= 0)
        {
            return NotFound(new UpdateJobResponseDto()
            {
                Message = "Invalid request data",
                StatusCode = StatusCodes.Status400BadRequest,
                Success = false,
            });
        }

        var command = new UpdateJobCommand(updateRequest, id);
        var response = await _mediator.Send(command);

        return !response.Success ? BadRequest(response) : Ok(response);
    }

}