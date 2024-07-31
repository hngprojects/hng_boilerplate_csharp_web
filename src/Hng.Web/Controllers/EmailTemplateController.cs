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
    private readonly ISender sender = sender;


    /// <summary>
    /// Create an email template
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(EmailTemplateDTO), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(FailureResponseDto<string>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CreateTemplate([FromBody] CreateEmailTemplateDTO createEmailDTO)
    {
        CreateEmailTemplateCommand createEmailTemplateCommand = new(createEmailDTO);
        EmailTemplateDTO createdTemplate = await sender.Send(createEmailTemplateCommand);

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
        return Ok(successResponseDto);
    }

    /// <summary>
    /// gets all the email templates in the system
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<EmailTemplateDTO>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllTemplates()
    {
        GetAllEmailTemplatesQuery query = new();
        IEnumerable<EmailTemplateDTO> templates = await sender.Send(query);
        SuccessResponseDto<IEnumerable<EmailTemplateDTO>> successResponseDto = new() { Data = templates };
        return Ok(successResponseDto);
    }
}
