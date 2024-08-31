using Hng.Application.Features.Faq.Dtos;
using Hng.Application.Features.Faq.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Hng.Graphql
{
    public partial class Queries
    {
        public async Task<List<FaqResponseDto>> GetAllFaqs([FromServices] IMediator mediator)
        {
            var query = new GetAllFaqsQuery();
            return await mediator.Send(query);
        }
    }
}
