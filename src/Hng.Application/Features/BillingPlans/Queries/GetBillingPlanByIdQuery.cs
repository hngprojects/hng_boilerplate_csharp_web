using Hng.Application.Features.BillingPlans.Dtos;
using Hng.Application.Shared.Dtos;
using MediatR;

namespace Hng.Application.Features.BillingPlans.Queries
{
    public class GetBillingPlanByIdQuery : IRequest<SuccessResponseDto<BillingPlanDto>>
    {
        public Guid Id { get; }
        public GetBillingPlanByIdQuery(Guid id)
        {
            Id = id;
        }
    }
}