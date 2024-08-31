using Hng.Application.Features.ContactsUs.Command;
using Hng.Application.Features.ContactsUs.Dtos;
using HotChocolate.Authorization;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Hng.Graphql
{
    public partial class Mutations
    {
        public async Task<ContactResponse<ContactUsResponseDto>> CreateContactMessage(ContactUsRequestDto contactUsRequest, [FromServices] IMediator mediator)
        {
            var command = new CreateContactUsCommand(contactUsRequest);
            return await mediator.Send(command);
        }

        [Authorize]
        public async Task<ContactResponse<object>> DeleteContactMessage(Guid id, [FromServices] IMediator mediator)
        {
            var command = new DeleteContactUsCommand(id);
            return await mediator.Send(command);
        }
    }
}
