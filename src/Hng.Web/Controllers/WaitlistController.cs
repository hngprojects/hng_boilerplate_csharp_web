using Hng.Application.Models.WaitlistModels;
using Hng.Application.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Hng.API.Controllers
{
    [ApiController]
    [Route("v1/api")]
    public class WaitlistController : ControllerBase
    {
        private readonly IWaitlistService _waitlistService;

        public WaitlistController(IWaitlistService waitlistService)
        {
            _waitlistService = waitlistService;
        }

        [HttpPost("waitlist")]
        public async Task<IActionResult> SignUp([FromBody] WaitlistUserRequestModel model)
        {
            var result = await _waitlistService.SignUpAsync(model);

            if (!result.Success)
            {
                return StatusCode(result.StatusCode, new { message = result.Message, error = result.Error });
            }

            return StatusCode(result.StatusCode, new { message = result.Message });
        }
    }
}
