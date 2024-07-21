using AutoMapper;
using Hng.Application.Dto;
using Hng.Application.Interfaces;
using Hng.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hng.Web.Controllers
{
    [ApiController]
    [Route("api/v1/plans")]
    [Authorize(Roles = "Admin")]
    public class SubscriptionPlanController : ControllerBase
    {
        private readonly ISubscriptionPlanService _service;
        private readonly ILogger<SubscriptionPlanService> _logger;

        public SubscriptionPlanController(ISubscriptionPlanService service, ILogger<SubscriptionPlanService> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> CreatePlan([FromBody] CreateSubscriptionPlanDto dto)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return StatusCode(403, new { status = 403, error = "User is not authorized to create subscription plans." });
            }
            try
            {
                var result = await _service.CreatePlanAsync(dto);
                return Ok(new { data = result, status = 200 });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { status = 400, error = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { status = 401, error = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(400, new { status = 400, error = ex.Message });
            }
        }
        // => Ok(await _service.CreatePlanAsync(dto));
    }
}
