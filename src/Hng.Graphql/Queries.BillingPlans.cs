using Hng.Application.Features.BillingPlans.Dtos;
using Hng.Application.Features.BillingPlans.Queries;
using Hng.Application.Shared.Dtos;
using HotChocolate.Authorization;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Hng.Graphql
{
    public partial class Queries
    {
        [Authorize]
        public async Task<SuccessResponseDto<BillingPlanDto>> GetBillingPlanById(Guid id, [FromServices] IMediator mediator)
        {
            var result = new GetBillingPlanByIdQuery(id);
            return await mediator.Send(result);
        }

        [Authorize]
        public async Task<PaginatedResponseDto<List<BillingPlanDto>>> GetAllBillingPlans(BaseQueryParameters queryParameters, [FromServices] IMediator mediator)
        {
            var query = new GetAllBillingPlansQuery(queryParameters);
            return await mediator.Send(query);
        }
    }
}