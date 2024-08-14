using Hng.Application.Features.Dashboard.Dtos;
using Hng.Application.Features.Dashboard.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hng.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public DashboardsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> GetUserProduct([FromQuery] Guid userId)
        {
            var response = await _mediator.Send(new GetDashboardQuery(userId));
            if (response != null)
            {
                return Ok(new
                {
                    data = response,
                    message = "Retrieved successfully",
                    status_code = 200
                });

            }
            return NotFound(new
            {
                error = "No record found for this user",
                message = "Request failed",
                status_code = 404
            });
        }

        [HttpGet("sales-trend")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> GetSalesTrend([FromQuery] SalesTrendQueryParameter parameter)
        {
            var response = await _mediator.Send(new GetSalesTrendQuery(parameter));
            if (response != null)
            {
                return Ok(new
                {
                    data = response,
                    message = "Retrieved successfully",
                    status_code = 200
                });

            }
            return NotFound(new
            {
                error = "No record found",
                message = "Request failed",
                status_code = 404
            });
        }
    }
}
