using Hng.Application.Features.HelpCenter.Command;
using Hng.Application.Features.HelpCenter.Dtos;
using Hng.Application.Features.HelpCenter.Queries;
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

        [HttpPost]
        public async Task<IActionResult> CreateHelpCenterTopic([FromBody] CreateHelpCenterTopicRequestDto request)
        {
            var command = new CreateHelpCenterTopicCommand(request);
            var result = await _mediator.Send(command);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetHelpCenterTopicById(Guid id)
        {
            var query = new GetHelpCenterTopicByIdQuery(id);
            var result = await _mediator.Send(query);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet]
        public async Task<IActionResult> GetHelpCenterTopics()
        {
            var query = new GetHelpCenterTopicsQuery();
            var result = await _mediator.Send(query);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateHelpCenterTopic(Guid id, [FromBody] UpdateHelpCenterTopicRequestDto request)
        {
            var command = new UpdateHelpCenterTopicCommand(id, request);
            var result = await _mediator.Send(command);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHelpCenterTopic(Guid id)
        {
            var command = new DeleteHelpCenterTopicCommand(id);
            var result = await _mediator.Send(command);
            return StatusCode(result.StatusCode, result);
        }
        [HttpGet("search")]
        public async Task<IActionResult> SearchTopics([FromQuery] SearchHelpCenterTopicsRequestDto request)
        {
            var query = new SearchHelpCenterTopicsQuery(request);
            var result = await _mediator.Send(query);
            return StatusCode(result.StatusCode, result);
        }

    }

}
