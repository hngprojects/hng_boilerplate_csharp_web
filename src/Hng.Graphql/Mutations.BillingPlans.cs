using Hng.Application.Features.BillingPlans.Commands;
using Hng.Application.Features.BillingPlans.Dtos;
using Hng.Application.Features.BillingPlans.Queries;
using Hng.Application.Shared.Dtos;
using HotChocolate.Authorization;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Hng.Graphql
{
    public partial class Mutations
    {
        [Authorize]
        public async Task<SuccessResponseDto<BillingPlanDto>> CreateBillingPlan(CreateBillingPlanDto createBillingPlanDto, [FromServices] IMediator mediator)
        {
            var result = new CreateBillingPlanCommand(createBillingPlanDto);
            return await mediator.Send(result);
        }

        [Authorize]
        public async Task<SuccessResponseDto<BillingPlanDto>> UpdateBillingPlan(Guid id, CreateBillingPlanDto updateBillingPlanDto, [FromServices] IMediator mediator)
        {
            var result = new UpdateBillingPlanCommand(id, updateBillingPlanDto);
            return await mediator.Send(result);
        }

        [Authorize]
        public async Task<SuccessResponseDto<bool>> DeleteBillingPlan(Guid id, [FromServices] IMediator mediator)
        {
            var result = new DeleteBillingPlanCommand(id);
            return await mediator.Send(result);
        }
    }
}