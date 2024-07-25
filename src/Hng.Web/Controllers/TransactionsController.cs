using Hng.Application.Features.PaymentIntegrations.Paystack.Dtos.Requests;
using Hng.Application.Features.PaymentIntegrations.Paystack.Dtos.Responses;
using Hng.Infrastructure.Utilities.StringKeys;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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

        /// <summary>
        /// Initiaze transation from Paystack
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("initialize")]
        [ProducesResponseType(typeof(InitializeTransactionResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> InitializeTransaction([FromBody] InitializeTransactionCommand command)
        {
            var result = await _mediator.Send(command);

            if (result.IsSuccess)
                return Ok(result.Value);

            return BadRequest(result.Error);
        }

        /// <summary>
        /// Used to Verify Transaction from Paystack
        /// </summary>
        /// <param name="reference"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("verify/{reference}")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> VerifyTransaction(string reference)
        {
            var response = await _mediator.Send(new VerifyTransactionQuery(reference));

            if (response.IsFailure)
                return BadRequest(response.Error);

            return Ok(response);
        }

        /// <summary>
        /// Used to listen to Paystack webhook event
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("callback")]
        public async Task<IActionResult> GetTransferStatsusForRecipients([FromBody] dynamic content)
        {
            var data = JsonConvert.DeserializeObject<TransactionSuccessfulCommand>(content.ToString());

            if (data.Event == PaystackEventKeys.charge_success)
                await _mediator.Send(data);

            return Ok(new { Status = true, Message = "success" });
        }
    }
}