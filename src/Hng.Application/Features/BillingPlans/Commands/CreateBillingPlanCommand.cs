using Hng.Application.Features.BillingPlans.Dtos;
using Hng.Application.Shared.Dtos;
using MediatR;

namespace Hng.Application.Features.BillingPlans.Commands
{
    public class CreateBillingPlanCommand : IRequest<SuccessResponseDto<BillingPlanDto>>
    {
        public CreateBillingPlanDto BillingPlanDto { get; }
        public CreateBillingPlanCommand(CreateBillingPlanDto billingPlanDto)
        {
            BillingPlanDto = billingPlanDto;
        }
    }
}