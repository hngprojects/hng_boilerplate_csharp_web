using Hng.Application.Dto;
using Hng.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Hng.Web.Controllers
{
    [Route("api/v1/news-letter")]
    [ApiController]
    public class NewsLetterController : ControllerBase
    {
        private readonly INewsLetterSubscriptionService _newsLetterSubscriptionService;

        public NewsLetterController(INewsLetterSubscriptionService newsLetterSubscriptionService)
        {
            _newsLetterSubscriptionService = newsLetterSubscriptionService;
        }
        [HttpPost]
        [EndpointDescription("Subscribe To News Letter")]
        [ProducesResponseType<SuccessResponseDto>((int)HttpStatusCode.Created)]
        [ProducesResponseType<FailureResponseDto>((int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> RegisterSubscriber(NewsLetterSubscriptionDto subscriber)
        {
            var result = await _newsLetterSubscriptionService.AddToNewsLetter(subscriber);
            if (result is null)
                return Unauthorized(new FailureResponseDto("Email already exists.", (int)HttpStatusCode.Unauthorized));
            return StatusCode((int)HttpStatusCode.Created, new SuccessResponseDto("Email was successfully stored.", (int)HttpStatusCode.Created));
        }
    }
}
