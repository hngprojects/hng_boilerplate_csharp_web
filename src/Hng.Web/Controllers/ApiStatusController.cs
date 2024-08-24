using Hng.Application.Features.ApiStatuses.Dtos.Requests;
using Hng.Application.Features.ApiStatuses.Dtos.Responses;
using Hng.Application.Features.Profiles.Dtos;
using Hng.Application.Shared.Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Hng.Web.Controllers
{
    [ApiController]
    [Route("api/v1/api-status")]
    public class ApiStatusController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ApiStatusController(IMediator mediator) => _mediator = mediator;

        /// <summary>
        /// Retrieves all APIStatus entries.
        /// </summary>
        /// <returns>A list of all APIStatus with a success message.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(PaginatedResponseDto<List<ApiStatusResponseDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(FailureResponseDto<string>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllApiStatuses([FromQuery] GetAllApiStatusesQueries query)
        {
            var result = await _mediator.Send(query);

            if (result.Data.Count > 0)
            {
                return Ok(new
                {
                    Status = 200,
                    Message = "APIs retrieved successfully",
                    Data = new
                    {
                        Message = "Request completed successfully.",
                        result.Metadata,
                        result.Data
                    }
                });
            }

            return NotFound(new
            {
                Status = 404,
                Message = "No APIs found."
            });
        }

        /// <summary>
        /// updates api statuses
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(CreateApiStatusResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(CreateApiStatusResponseDto), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateApiStatuses([FromForm] UpdateApiStatusDto command)
        {
            var response = await _mediator.Send(command);

            if (response.IsFailure)
                return StatusCode(StatusCodes.Status400BadRequest,
                new UpdateProfileResponseDto()
                {
                    Message = response.Error,
                    StatusCode = StatusCodes.Status400BadRequest
                });

            return Ok(response.Value);
        }
    }
}