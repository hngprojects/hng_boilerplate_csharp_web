using Hng.Application.Features.Subscriptions.Dtos.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Hng.Web.Controllers
{
    [ApiController]
    [Route("api/v1/subscriptions")]
    public class SubscriptionsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SubscriptionsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// To subscribe for Free Plan
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public async Task<IActionResult> SubscribeFreePlan([FromBody] SubscribeFreePlan command)
        {
            var result = await _mediator.Send(command);

            if (result.IsSuccess)
                return Ok(result.Value);

            return BadRequest(result.Error);
        }
    }
}
