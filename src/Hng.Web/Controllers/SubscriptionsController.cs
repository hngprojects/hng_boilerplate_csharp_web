using Hng.Application.Features.Subscriptions.Commands;
using Hng.Application.Features.Subscriptions.Dtos;
using Hng.Application.Features.Subscriptions.Dtos.Requests;
using Hng.Application.Features.Subscriptions.Dtos.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize]
        [HttpPost("free")]
        [ProducesResponseType(typeof(SubscribeFreePlanResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> SubscribeFreePlan([FromBody] SubscribeFreePlan command)
        {
            var result = await _mediator.Send(command);

            if (result.IsSuccess)
                return Ok(result.Value);

            return BadRequest(result.Error);
        }

        [HttpPost("{subscriptionId}/activate")]
        [Authorize]
        [ProducesResponseType(typeof(SubscriptionDto), StatusCodes.Status200OK)]
        public async Task<ActionResult> ActivateSubscription(Guid subscriptionId)
        {
            var command = new ActivateSubscriptionCommand(subscriptionId);
            var activateSubcription = await _mediator.Send(command);
            if (activateSubcription != null)
            {
                return Ok(new
                {
                    data = activateSubcription,
                    message = "Subscription activated successfully"
                });

            }
            return NotFound(new
            {
                error = "Subscription not found. Please check the subscription ID and try again.",
                message = "Request failed"
            });
        }
    }
}
