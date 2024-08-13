using Hng.Application.Features.BillingPlans.Dtos;
using Hng.Application.Shared.Dtos;
using MediatR;

namespace Hng.Application.Features.BillingPlans.Commands
{
    public class UpdateBillingPlanCommand : IRequest<SuccessResponseDto<BillingPlanDto>>
    {
        public Guid Id { get; }
        public CreateBillingPlanDto BillingPlanDto { get; }

        public UpdateBillingPlanCommand(Guid id, CreateBillingPlanDto billingPlanDto)
        {
            Id = id;
            BillingPlanDto = billingPlanDto;
        }
    }
}