using Hng.Application.Features.HelpCenter.Command;
using Hng.Application.Features.HelpCenter.Dtos;
using HotChocolate.Authorization;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Hng.Graphql
{
    public partial class Mutations
    {
        [Authorize]
        public async Task<HelpCenterResponseDto<HelpCenterTopicResponseDto>> CreateHelpCenterTopic([FromBody] CreateHelpCenterTopicRequestDto request, [FromServices] IMediator mediator)
        {
            var command = new CreateHelpCenterTopicCommand(request);
            return await mediator.Send(command);
        }

        [Authorize]
        public async Task<HelpCenterResponseDto<HelpCenterTopicResponseDto>> UpdateHelpCenterTopic(Guid id, UpdateHelpCenterTopicRequestDto request, [FromServices] IMediator mediator)
        {
            var command = new UpdateHelpCenterTopicCommand(id, request);
            return await mediator.Send(command);
        }

        [Authorize]
        public async Task<HelpCenterResponseDto<object>> DeleteHelpCenterTopic(Guid id, [FromServices] IMediator mediator)
        {
            var command = new DeleteHelpCenterTopicCommand(id);
            return await mediator.Send(command);
        }
    }
}
