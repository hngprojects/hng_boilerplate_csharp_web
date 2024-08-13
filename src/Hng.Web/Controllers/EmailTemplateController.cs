using Hng.Application.Features.EmailTemplates.Commands;
using Hng.Application.Features.EmailTemplates.DTOs;
using Hng.Application.Features.EmailTemplates.Queries;
using Hng.Application.Shared.Dtos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hng.Web.Controllers;

[Authorize]
[Route("/api/v1/[controller]")]
public class EmailTemplateController(ISender sender) : ControllerBase
{
    private readonly ISender _sender = sender;


    /// <summary>
    /// Create an email template
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(SuccessResponseDto<EmailTemplateDTO>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(FailureResponseDto<string>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CreateTemplate([FromBody] CreateEmailTemplateDTO createEmailDTO)
    {
        CreateEmailTemplateCommand createEmailTemplateCommand = new(createEmailDTO);
        EmailTemplateDTO createdTemplate = await _sender.Send(createEmailTemplateCommand);

        if (createdTemplate == null)
        {
            return Conflict(new FailureResponseDto<string>
            {
                Data = string.Empty,
                Error = "Template already exists",
                Message = "A template with the specified name already exists in the system"
            });
        };

        SuccessResponseDto<EmailTemplateDTO> successResponseDto = new() { Data = createdTemplate };
        return new ObjectResult(successResponseDto) { StatusCode = StatusCodes.Status201Created };
    }

    /// <summary>
    /// gets all the email templates in the system
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(SuccessResponseDto<PaginatedResponseDto<PagedListDto<EmailTemplateDTO>>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(FailureResponseDto<string>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAllTemplates([FromQuery] GetAllEmailTemplatesQuery query)
    {

        var templates = await _sender.Send(query);
        if (templates == null)
        {
            return BadRequest(new FailureResponseDto<string>()
            {
                Data = string.Empty,
                Error = "Invalid parameters entered",
                Message = "The Page numbers entered are not valid"
            });
        }

        SuccessResponseDto<PaginatedResponseDto<PagedListDto<EmailTemplateDTO>>> successResponseDto = new() { Data = templates };
        return Ok(successResponseDto);
    }
}
