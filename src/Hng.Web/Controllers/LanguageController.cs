using Hng.Application.Features.Languages.Dtos;
using Hng.Application.Features.Languages.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Hng.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LanguageController : ControllerBase
    {
        private readonly IMediator _mediator;

        public LanguageController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("")]
        [ProducesResponseType(typeof(IEnumerable<LanguageDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<LanguageDto>>> GetLanguages()
        {
            var languages = await _mediator.Send(new GetAllLanguagesQuery());
            return Ok(languages);
        }
    }
}