using Hng.Application.Features.Timezones.Commands;
using Hng.Application.Features.Timezones.Dtos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hng.Web.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/v1/timezones")]
    public class TimezoneController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TimezoneController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Create a new timezone.
        /// </summary>
        /// <param name="command">The create timezone command.</param>
        /// <returns>The created timezone.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(TimezoneDto), StatusCodes.Status201Created)]
        public async Task<ActionResult<TimezoneDto>> CreateTimezone([FromBody] CreateTimezoneCommand command)
        {
            var response = await _mediator.Send(command);
            return CreatedAtAction(nameof(CreateTimezone), response);
        }

        /// <summary>
        /// Update an existing timezone.
        /// </summary>
        /// <param name="id">The ID of the timezone to update.</param>
        /// <param name="command">The update timezone command.</param>
        /// <returns>The updated timezone.</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(TimezoneDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateTimezone(Guid id, [FromBody] UpdateTimezoneCommand command)
        {
            command.Id = id;
            return Ok(await _mediator.Send(command));
        }
    }
}