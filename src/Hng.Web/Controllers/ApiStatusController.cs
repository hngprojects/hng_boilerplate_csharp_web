using Hng.Application.Features.ApiStatuses.Commands;
using Hng.Application.Features.ApiStatuses.Dtos.Requests;
using Hng.Application.Features.ApiStatuses.Dtos.Responses;
using Hng.Application.Shared.Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Hng.Web.Controllers
{
    [Route("api-status")]
    [ApiController]
    public class ApiStatusController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ApiStatusController(IMediator mediator)
        {
            _mediator = mediator;
        }


        /// <summary>
        /// Uploads a JSON file containing API statuses.
        /// </summary>
        /// <param name="file">The JSON file containing API statuses.</param>
        /// <returns>A response indicating the result of the operation.</returns>
        [HttpPost("upload")]
        [ProducesResponseType(typeof(CreateApiStatusResponseDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> CreateApiStatus(IFormFile file)
        {
            var command = new CreateApiStatusCommand { File = file };
            var result = await _mediator.Send(command);
            return StatusCode(result.StatusCode, result);
        }


        /// <summary>
        /// Retrieves all APIStatus entries.
        /// </summary>
        /// <returns>A list of all APIStatus with a success message.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(ApiStatusResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(FailureResponseDto<string>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllFaqs()
        {
            var query = new GetAllApiStatusesQueries();

            var result = await _mediator.Send(query);

            if (result.Data.Any())
            {
                return Ok(new
                {
                    Status = 200,
                    Message = "APIs retrieved successfully",
                    Data = new
                    {
                        Message = "Request completed successfully.",
                        Metadata = result.Metadata,
                        Data = result.Data
                    }
                });
            }
            else
            {
                return NotFound(new
                {
                    Status = 404,
                    Message = "No APIs found."
                });
            }
        }
    }
}