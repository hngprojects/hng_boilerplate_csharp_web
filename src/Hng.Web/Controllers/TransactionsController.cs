using Hng.Application.Features.PaymentIntegrations.Paystack.Dtos.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Hng.Web.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class TransactionsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TransactionsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{reference}")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> VerifyTransaction(string reference)
        {
            var response = await _mediator.Send(new VerifyTransactionQuery(reference));

            if (response.IsFailure)
                return BadRequest(response.Error);

            return Ok(response);
        }
    }
}
