using Hng.Application.Features.HelpCenter.Dtos;
using Hng.Application.Features.HelpCenter.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Hng.Graphql
{
    public partial class Queries
    {
        public async Task<HelpCenterResponseDto<HelpCenterTopicResponseDto>> GetHelpCenterTopicById(Guid id, [FromServices] IMediator mediator)
        {
            var query = new GetHelpCenterTopicByIdQuery(id);
            return await mediator.Send(query);
        }

        public async Task<HelpCenterResponseDto<List<HelpCenterTopicResponseDto>>> GetHelpCenterTopics([FromServices] IMediator mediator)
        {
            var query = new GetHelpCenterTopicsQuery();
            return await mediator.Send(query);
        }

        public async Task<HelpCenterResponseDto<List<HelpCenterTopicResponseDto>>> SearchTopics(SearchHelpCenterTopicsRequestDto request, [FromServices] IMediator mediator)
        {
            var query = new SearchHelpCenterTopicsQuery(request);
            return await mediator.Send(query);
        }
    }
}
