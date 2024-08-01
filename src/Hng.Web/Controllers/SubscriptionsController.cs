using Hng.Application.Features.Subscriptions.Commands;
using Hng.Application.Features.Subscriptions.Dtos.Requests;
using Hng.Application.Features.Subscriptions.Dtos.Responses;
using Hng.Application.Features.Subscriptions.Queries;
using Hng.Application.Shared.Dtos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hng.Web.Controllers
{
    [Authorize]
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
       // [Authorize]
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

        /// <summary>
        /// Get subscription by user ID.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet("user/{userId}")]
        [ProducesResponseType(typeof(SuccessResponseDto<SubscriptionDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(FailureResponseDto<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetSubscriptionByUserId(Guid userId)
        {
            var response = await _mediator.Send(new GetSubscriptionByUserIdQuery(userId));
            return response != null
                ? Ok(new SuccessResponseDto<SubscriptionDto> { Data = response })
                : NotFound(new FailureResponseDto<object> { Error = "Subscription not found", Data = false });
        }

        /// <summary>
        /// Get subscription by organisation ID.
        /// </summary>
        /// <param name="organizationId"></param>
        /// <returns></returns>
        [HttpGet("organization/{organizationId}")]
        [ProducesResponseType(typeof(SuccessResponseDto<SubscriptionDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(FailureResponseDto<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetSubscriptionByOrganizationId(Guid organizationId)
        {
            var response = await _mediator.Send(new GetSubscriptionByOrganizationIdQuery(organizationId));
            return response != null
                ? Ok(new SuccessResponseDto<SubscriptionDto> { Data = response })
                : NotFound(new FailureResponseDto<object> { Error = "Organization not found", Data = false });
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

        /// <summary>
        /// Get subscriptions (Paginated)
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
		[HttpGet]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> GetSubscriptions([FromQuery] GetSubscriptionsQueryParameters parameters)
        {
            var subscriptions = await _mediator.Send(new GetSubscriptionsQuery(parameters));
            return Ok(new PaginatedResponseDto<PagedListDto<SubscriptionDto>> { Data = subscriptions, Metadata = subscriptions.MetaData });
        }
    }
}