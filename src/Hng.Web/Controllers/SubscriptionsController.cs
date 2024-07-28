using Hng.Application.Features.Subscriptions.Commands;
using Hng.Application.Features.Subscriptions.Dtos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hng.Web.Controllers
{
	[Authorize]
	[Route("api/v1/[controller]")]
	[ApiController]
	public class SubscriptionsController : ControllerBase
	{
		private readonly IMediator _mediator;
		public SubscriptionsController(IMediator mediator)
		{
			_mediator = mediator;
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
