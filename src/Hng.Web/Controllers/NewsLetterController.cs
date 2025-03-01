using System.Net;
using Hng.Application.Features.NewsLetterSubscription.Commands;
using Hng.Application.Features.NewsLetterSubscription.Dtos;
using Hng.Application.Features.NewsLetterSubscription.Queries;
using Hng.Application.Shared.Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Hng.Web.Controllers
{
    [ApiController]
    [Route("api/v1/news-letter")]
    public class NewsLetterController : ControllerBase
    {
        private readonly IMediator _mediator;
        public NewsLetterController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [EndpointDescription("Subscribe To News Letter")]
        [ProducesResponseType<SuccessResponseDto<NewsLetterSubscriptionDto>>((int)HttpStatusCode.Created)]
        [ProducesResponseType<FailureResponseDto<object>>((int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> RegisterSubscriber(NewsLetterSubscriptionDto subscriber)
        {
            var result = await _mediator.Send(new AddSubscriberCommand(subscriber));
            if (result is null)
                return Unauthorized(new FailureResponseDto<object> { Message = "Email already exists.", Error = StatusCodes.Status401Unauthorized.ToString(), Data = null });
            return StatusCode((int)HttpStatusCode.Created, new SuccessResponseDto<NewsLetterSubscriptionDto> { Message = "Email was successfully stored.", Data = result });
        }

        [HttpGet("subscribers")]
        public async Task<IActionResult> GetSubscribers([FromQuery] int page = 1, [FromQuery] int limit = 10)
        {
            var query = new GetPaginatedSubscribersQuery { Page = page, Limit = limit };
            var result = await _mediator.Send(query);

            return StatusCode(result.StatusCode, result);
        }




    }
}