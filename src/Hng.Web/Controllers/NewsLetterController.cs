using Hng.Application.Dto;
using Hng.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<ActionResult<NewsLetterSubscriptionDto>> RegisterSubscriber(NewsLetterSubscriptionDto subscriber)
        {
            var result = await _newsLetterSubscriptionService.AddToNewsLetter(subscriber);
            if (result is null)
                return BadRequest("Subscriber Already Exists");
            return Ok(result);

        }
    }
}
