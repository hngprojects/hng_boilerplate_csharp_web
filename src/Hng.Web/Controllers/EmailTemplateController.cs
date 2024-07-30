using Hng.Application.Features.EmailTemplates.Commands;
using Hng.Application.Features.EmailTemplates.DTOs;
using Hng.Application.Features.EmailTemplates.Queries;
using Hng.Application.Shared.Dtos;
using Hng.Domain.Entities;
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
    /// gets all the email templates in the system
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(EmailTemplateDTO), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllTemplates()
    {
        GetAllEmailTemplatesQuery query = new();
        IEnumerable<EmailTemplateDTO> templates = await sender.Send(query);
        SuccessResponseDto<IEnumerable<EmailTemplateDTO>> successResponseDto = new() { Data = templates };
        return Ok(successResponseDto);
    }
}
