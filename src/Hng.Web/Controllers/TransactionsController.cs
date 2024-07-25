﻿using Hng.Application.Features.PaymentIntegrations.Paystack.Dtos.Requests;
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