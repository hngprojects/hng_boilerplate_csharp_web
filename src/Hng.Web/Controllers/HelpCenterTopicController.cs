using Hng.Application.Features.HelpCenter.Command;
using Hng.Application.Features.HelpCenter.Dtos;
using Hng.Application.Features.HelpCenter.Queries;
using Hng.Application.Shared.Dtos;
using HotChocolate.Authorization;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hng.Web.Controllers
{
    [ApiController]
    [Route("api/v1/help-center/topics")]
    public class HelpCenterTopicController : ControllerBase
    {
        private readonly IMediator _mediator;

        public HelpCenterTopicController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Creates a new Help Center topic.
        /// </summary>
        /// <param name="request">The details of the Help Center topic to create.</param>
        /// <returns>A response with the creation result or an error message.</returns>
        [HttpPost]
        [Authorize]
        [ProducesResponseType(typeof(HelpCenterResponseDto<HelpCenterTopicResponseDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(FailureResponseDto<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateHelpCenterTopic([FromBody] CreateHelpCenterTopicRequestDto request)
        {
            var command = new CreateHelpCenterTopicCommand(request);
            var result = await _mediator.Send(command);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Retrieves a specific Help Center topic by ID.
        /// </summary>
        /// <param name="id">The ID of the Help Center topic.</param>
        /// <returns>A response with the topic details or an error message.</returns>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(HelpCenterResponseDto<HelpCenterTopicResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(FailureResponseDto<string>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetHelpCenterTopicById(Guid id)
        {
            var query = new GetHelpCenterTopicByIdQuery(id);
            var result = await _mediator.Send(query);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Retrieves all Help Center topics.
        /// </summary>
        /// <returns>A list of all Help Center topics with a success message.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(HelpCenterResponseDto<List<HelpCenterTopicResponseDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(FailureResponseDto<string>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetHelpCenterTopics()
        {
            var query = new GetHelpCenterTopicsQuery();
            var result = await _mediator.Send(query);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Updates an existing Help Center topic.
        /// </summary>
        /// <param name="id">The ID of the Help Center topic to update.</param>
        /// <param name="request">The updated Help Center topic details.</param>
        /// <returns>A response with the update result or an error message.</returns>
        [HttpPatch("{id:guid}")]
        [ProducesResponseType(typeof(HelpCenterResponseDto<HelpCenterTopicResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(FailureResponseDto<string>), StatusCodes.Status404NotFound)]

        public async Task<IActionResult> UpdateHelpCenterTopic(Guid id, [FromBody] UpdateHelpCenterTopicRequestDto request)
        {
            var command = new UpdateHelpCenterTopicCommand(id, request);
            var result = await _mediator.Send(command);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Deletes a Help Center topic.
        /// </summary>
        /// <param name="id">The ID of the Help Center topic to delete.</param>
        /// <returns>A response with the deletion result or an error message.</returns>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(typeof(SuccessResponseDto<object>), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(FailureResponseDto<string>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteHelpCenterTopic(Guid id)
        {
            var command = new DeleteHelpCenterTopicCommand(id);
            var result = await _mediator.Send(command);
            return StatusCode(result.StatusCode, result);
        }
        /// <summary>
        /// Searches Help Center topics based on title and content.
        /// </summary>
        /// <param name="request">The search criteria for Help Center topics.</param>
        /// <returns>A list of topics that match the search criteria.</returns>
        [HttpGet("search")]
        [ProducesResponseType(typeof(HelpCenterResponseDto<List<HelpCenterTopicResponseDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(FailureResponseDto<string>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> SearchTopics([FromQuery] SearchHelpCenterTopicsRequestDto request)
        {
            var query = new SearchHelpCenterTopicsQuery(request);
            var result = await _mediator.Send(query);
            return StatusCode(result.StatusCode, result);
        }

    }

}
