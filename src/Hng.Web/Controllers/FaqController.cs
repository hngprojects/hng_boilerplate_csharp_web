using Hng.Application.Features.Faq.Commands;
using Hng.Application.Features.Faq.Dtos;
using Hng.Application.Features.Faq.Queries;
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

    [HttpPost]
    public async Task<IActionResult> CreateFaq([FromBody] CreateFaqRequestDto faqRequest)
    {
        var command = new CreateFaqCommand(faqRequest);
        var result = await _mediator.Send(command);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateFaq(Guid id, [FromBody] UpdateFaqRequestDto faqRequest)
    {
        var command = new UpdateFaqCommand(id, faqRequest);
        var result = await _mediator.Send(command);
        return StatusCode(result.StatusCode, result);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteFaq(Guid id)
    {
        var command = new DeleteFaqCommand(id);
        var result = await _mediator.Send(command);
        return StatusCode(result.StatusCode, result);
    }
    [HttpGet]
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

