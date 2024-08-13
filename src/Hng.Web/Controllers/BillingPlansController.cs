using Hng.Application.Features.BillingPlans.Commands;
using Hng.Application.Features.BillingPlans.Dtos;
using Hng.Application.Features.BillingPlans.Queries;
using Hng.Application.Shared.Dtos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hng.Web.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/v1/billing-plans")]
    public class BillingPlansController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BillingPlansController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Creates a new billing plan
        /// </summary>
        /// <param name="createBillingPlanDto">The billing plan details</param>
        /// <returns>The created billing plan</returns>
        [HttpPost]
        [ProducesResponseType(typeof(SuccessResponseDto<BillingPlanDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(FailureResponseDto<string>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<SuccessResponseDto<BillingPlanDto>>> CreateBillingPlan([FromBody] CreateBillingPlanDto createBillingPlanDto)
        {
            var result = await _mediator.Send(new CreateBillingPlanCommand(createBillingPlanDto));
            return result.Data != null ? CreatedAtAction(nameof(GetBillingPlanById), new { id = result.Data.Id }, result)
                                       : BadRequest(new FailureResponseDto<string> { Error = result.Message, Message = "Failed to create billing plan." });
        }

        /// <summary>
        /// Retrieves a billing plan by its ID
        /// </summary>
        /// <param name="id">The ID of the billing plan</param>
        /// <returns>The billing plan</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(SuccessResponseDto<BillingPlanDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(FailureResponseDto<string>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<SuccessResponseDto<BillingPlanDto>>> GetBillingPlanById(Guid id)
        {
            var result = await _mediator.Send(new GetBillingPlanByIdQuery(id));
            return result.Data != null ? Ok(result) : NotFound(new FailureResponseDto<string> { Error = result.Message });
        }

        /// <summary>
        /// Updates an existing billing plan
        /// </summary>
        /// <param name="id">The ID of the billing plan to update</param>
        /// <param name="updateBillingPlanDto">The updated billing plan details</param>
        /// <returns>The updated billing plan</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(SuccessResponseDto<BillingPlanDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(FailureResponseDto<string>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<SuccessResponseDto<BillingPlanDto>>> UpdateBillingPlan(Guid id, [FromBody] CreateBillingPlanDto updateBillingPlanDto)
        {
            var result = await _mediator.Send(new UpdateBillingPlanCommand(id, updateBillingPlanDto));
            return result.Data != null ? Ok(result) : NotFound(new FailureResponseDto<string> { Error = result.Message });
        }

        /// <summary>
        /// Retrieves all billing plans
        /// </summary>
        /// <returns>A list of all billing plans</returns>
        [HttpGet]
        [ProducesResponseType(typeof(SuccessResponseDto<PaginatedResponseDto<List<BillingPlanDto>>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<SuccessResponseDto<PaginatedResponseDto<List<BillingPlanDto>>>>> GetAllBillingPlans([FromQuery] BaseQueryParameters queryParameters)
        {
            var query = new GetAllBillingPlansQuery(queryParameters);
            var response = await _mediator.Send(query);
            return Ok(new SuccessResponseDto<PaginatedResponseDto<List<BillingPlanDto>>> { Data = response });
        }

        /// <summary>
        /// Deletes a billing plan
        /// </summary>
        /// <param name="id">The ID of the billing plan to delete</param>
        /// <returns>No content</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(SuccessResponseDto<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(FailureResponseDto<string>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<SuccessResponseDto<bool>>> DeleteBillingPlan(Guid id)
        {
            var result = await _mediator.Send(new DeleteBillingPlanCommand(id));
            return result.Data ? Ok(result) : NotFound(new FailureResponseDto<string> { Error = result.Message });
        }
    }
}