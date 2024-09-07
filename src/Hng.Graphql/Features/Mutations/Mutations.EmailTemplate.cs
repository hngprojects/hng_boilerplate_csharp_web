using Hng.Application.Features.EmailTemplates.Commands;
using Hng.Application.Features.EmailTemplates.DTOs;
using HotChocolate.Authorization;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Hng.Graphql
{
    public partial class Mutations
    {
        [Authorize]
        public async Task<EmailTemplateDTO> CreateTemplate(CreateEmailTemplateDTO createEmailDTO, [FromServices] IMediator mediator)
        {
            var command = new CreateEmailTemplateCommand(createEmailDTO);
            return await mediator.Send(command);
        }
    }
}
