using Hng.Application.Features.ExternalIntegrations.PaymentIntegrations.Paystack.Dtos.Requests;
using Hng.Application.Features.ExternalIntegrations.PaymentIntegrations.Paystack.Dtos.Responses;
using Hng.Application.Shared.Dtos;
using Hng.Infrastructure.Utilities.StringKeys;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Hng.Web.Controllers
{
    [ApiController]
    [Route("api/v1/transactions")]
    public class TransactionsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TransactionsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Initiate product transation from Paystack
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("initiate/product")]
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
        /// Initiate subscription transation from Paystack
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("initiate/subscription")]
        [ProducesResponseType(typeof(InitiateSubscriptionTransactionResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> InitializeSubscriptionTransaction([FromBody] InitiateSubscriptionTransactionCommand command)
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
            var data = JsonConvert.DeserializeObject<TransactionsWebhookCommand>(content.ToString());
            if (data.Event == PaystackEventKeys.charge_success)
            {
                if (data.Data.Metadata.ToString().Contains(nameof(ProductInitialized.ProductId)))
                {
                    var command = new TransactionWebhookCommand(data);
                    await _mediator.Send(command);
                }
                else if (data.Data.Metadata.ToString().Contains(nameof(SubscriptionInitialized.SubId)))
                {
                    var command = new SubTransactionWebhookCommand(data);
                    await _mediator.Send(command);
                }
            }
            return Ok(new { Status = true, Message = "success" });
        }

        /// <summary>
        /// Get transactions by user ID.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("user/{userId}")]
        [ProducesResponseType(typeof(SuccessResponseDto<PagedListDto<TransactionDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(FailureResponseDto<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetTransactionsByUserId(Guid userId, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var response = await _mediator.Send(new GetTransactionsByUserIdQuery(userId, pageNumber, pageSize));
            return response.Any()
                ? Ok(new SuccessResponseDto<PagedListDto<TransactionDto>> { Data = response })
                : NotFound(new FailureResponseDto<object> { Error = "No transaction found for this user", Data = false });
        }

        /// <summary>
        /// Get transactions by product ID.
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("product/{productId}")]
        [ProducesResponseType(typeof(SuccessResponseDto<PagedListDto<TransactionDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(FailureResponseDto<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetTransactionsByProductId(Guid productId, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var response = await _mediator.Send(new GetTransactionsByProductIdQuery(productId, pageNumber, pageSize));
            return response.Any()
                ? Ok(new SuccessResponseDto<PagedListDto<TransactionDto>> { Data = response, Message = "Transactions retrieved successfully" })
                : NotFound(new FailureResponseDto<object> { Error = "No transaction found for this product", Data = false });
        }
    }
}