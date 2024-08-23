using Hng.Application.Features.Languages.Commands;
using Hng.Application.Features.Languages.Dtos;
using Hng.Application.Features.Languages.Queries;
using Hng.Application.Shared.Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Hng.Web.Controllers
{
   // [Authorize]
    [ApiController]
    [Route("api/v1/languages")]
    public class LanguagesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public LanguagesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Create a new language.
        /// </summary>
        /// <param name="command">The create language command.</param>
        /// <returns>The created language.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(SuccessResponseDto<LanguageDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(FailureResponseDto<object>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<SuccessResponseDto<LanguageDto>>> CreateLanguage([FromBody] CreateLanguageCommand command)
        {
            var response = await _mediator.Send(command);
            return CreatedAtAction(nameof(CreateLanguage), response);
        }

        /// <summary>
        /// Get a language by id.
        /// </summary>
        /// <param name="id">The language id.</param>
        /// <returns>The requested language.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(SuccessResponseDto<LanguageDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(FailureResponseDto<object>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<SuccessResponseDto<LanguageDto>>> GetLanguageById(Guid id)
        {
            var response = await _mediator.Send(new GetLanguageByIdQuery { Id = id });
            return Ok(response);
        }

        /// <summary>
        /// Get all languages.
        /// </summary>
        /// <param name="query">The query parameters.</param>
        /// <returns>A paginated list of languages.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(PaginatedResponseDto<List<LanguageDto>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<PaginatedResponseDto<List<LanguageDto>>>> GetAllLanguages([FromQuery] GetAllLanguagesQuery query)
        {
            var response = await _mediator.Send(query);
            return Ok(response);
        }

        /// <summary>
        /// Update a language.
        /// </summary>
        /// <param name="id">The language id.</param>
        /// <param name="command">The update language command.</param>
        /// <returns>The updated language.</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(SuccessResponseDto<LanguageDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(FailureResponseDto<object>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<SuccessResponseDto<LanguageDto>>> UpdateLanguage(Guid id, [FromBody] UpdateLanguageCommand command)
        {
            command.Id = id;
            var response = await _mediator.Send(command);
            return Ok(response);
        }

        /// <summary>
        /// Delete a language.
        /// </summary>
        /// <param name="id">The language id.</param>
        /// <returns>A success response if deleted.</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(SuccessResponseDto<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(FailureResponseDto<object>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<SuccessResponseDto<object>>> DeleteLanguage(Guid id)
        {
            var response = await _mediator.Send(new DeleteLanguageCommand { Id = id });
            return Ok(response);
        }
    }
}