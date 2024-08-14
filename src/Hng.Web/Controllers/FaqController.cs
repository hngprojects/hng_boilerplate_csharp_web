using Hng.Application.Features.Faq.Commands;
using Hng.Application.Features.Faq.Dtos;
using Hng.Application.Features.Faq.Queries;
using Hng.Application.Features.UserManagement.Dtos;
using Hng.Application.Shared.Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

[Route("api/v1/faqs")]
[ApiController]
public class FaqController : ControllerBase
{
    private readonly IMediator _mediator;

    public FaqController(IMediator mediator)
    {
        _mediator = mediator;
    }
    /// <summary>
    /// Creates a new FAQ entry.
    /// </summary>
    /// <param name="faqRequest">The details of the FAQ to create.</param>
    /// <returns>A response with the creation result or an error message.</returns>
    [HttpPost]
    [ProducesResponseType(typeof(CreateFaqResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(FailureResponseDto<string>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateFaq([FromBody] CreateFaqRequestDto faqRequest)
    {
        var command = new CreateFaqCommand(faqRequest);
        var result = await _mediator.Send(command);
        return StatusCode(result.StatusCode, result);
    }

    /// <summary>
    /// Updates an existing FAQ entry.
    /// </summary>
    /// <param name="id">The ID of the FAQ to update.</param>
    /// <param name="faqRequest">The updated FAQ details.</param>
    /// <returns>A response with the update result or an error message.</returns>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(UpdateFaqResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(FailureResponseDto<string>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateFaq(Guid id, [FromBody] UpdateFaqRequestDto faqRequest)
    {
        var command = new UpdateFaqCommand(id, faqRequest);
        var result = await _mediator.Send(command);
        return StatusCode(result.StatusCode, result);
    }


    /// <summary>
    /// Deletes an FAQ entry.
    /// </summary>
    /// <param name="id">The ID of the FAQ to delete.</param>
    /// <returns>A response with the deletion result or an error message.</returns>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(typeof(SuccessResponseDto<object>), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(FailureResponseDto<string>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteFaq(Guid id)
    {
        var command = new DeleteFaqCommand(id);
        var result = await _mediator.Send(command);
        return StatusCode(result.StatusCode, result);
    }
    /// <summary>
    /// Retrieves all FAQ entries.
    /// </summary>
    /// <returns>A list of all FAQs with a success message.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(FaqResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(FailureResponseDto<string>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAllFaqs()
    {
        var query = new GetAllFaqsQuery();
        var result = await _mediator.Send(query);
        return Ok(new
        {
            Status = 200,
            Message = "FAQs retrieved successfully",
            Data = result
        });
    }
}

