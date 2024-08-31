using Hng.Application.Features.Dashboard.Dtos;
using Hng.Application.Features.Dashboard.Queries;
using Hng.Application.Features.ExternalIntegrations.PaymentIntegrations.Paystack.Dtos.Responses;
using Hng.Application.Shared.Dtos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hng.Web.Controllers
{
    [Route("api/v1/[controller]")]
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
        [ProducesResponseType(typeof(DashboardDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(FailureResponseDto<object>), StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> GetUserProduct([FromQuery] Guid userId)
        {
            var response = await _mediator.Send(new GetDashboardQuery(userId));
            return Ok(new
            {
                data = response,
                message = "Retrieved successfully",
                status_code = 200
            });

        }

        [HttpGet("sales-trend")]
        [Authorize]
        [ProducesResponseType(typeof(PagedListDto<TransactionDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(FailureResponseDto<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(FailureResponseDto<object>), StatusCodes.Status401Unauthorized)]
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

        [HttpGet("overview-navigation-data")]
        [Authorize]
        [ProducesResponseType(typeof(NavigationDataDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(FailureResponseDto<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(FailureResponseDto<object>), StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> GetNavigationData()
        {
            var response = await _mediator.Send(new GetNavigationDataQuery());
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

        [HttpGet("export-sales-data")]
        [Authorize]
        [ProducesResponseType(typeof(List<TransactionDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(FailureResponseDto<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(FailureResponseDto<object>), StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> GetExportData([FromQuery] SalesTrendQueryParameter parameter)
        {
            var response = await _mediator.Send(new GetExportDataQuery(parameter));
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
