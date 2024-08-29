using Hng.Application.Features.ContactsUs.Command;
using Hng.Application.Features.ContactsUs.Dtos;
using Hng.Application.Features.ContactsUs.Queries;
using Hng.Application.Shared.Dtos;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hng.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactUsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ContactUsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Creates a new Contact Us message.
        /// </summary>
        /// <param name="contactUsRequest">The details of the message to create.</param>
        /// <returns>A response with the creation result or an error message.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(ContactResponse<ContactUsResponseDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(FailureResponseDto<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateContactMessage([FromBody] ContactUsRequestDto contactUsRequest)
        {
            var command = new CreateContactUsCommand(contactUsRequest);
            var result = await _mediator.Send(command);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Deletes an existing Contact Us message.
        /// </summary>
        /// <param name="id">The ID of the message to delete.</param>
        /// <returns>A response with the deletion result.</returns>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(typeof(SuccessResponseDto<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(FailureResponseDto<string>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteContactMessage(Guid id)
        {
            var command = new DeleteContactUsCommand(id);
            var result = await _mediator.Send(command);
            return StatusCode(result.StatusCode, result);
        }
        /// <summary>
        /// Gets all Contact Us messages.
        /// </summary>
        /// <returns>A list of Contact Us messages.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(ContactResponse<List<ContactUsResponseDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(FailureResponseDto<string>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllContactMessages()
        {
            var query = new GetAllContactUsQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }

    }
}
