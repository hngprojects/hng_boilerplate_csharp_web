using Hng.Application.Shared.Dtos;
using MediatR;

namespace Hng.Application.Features.BillingPlans.Commands
{
    public class DeleteBillingPlanCommand : IRequest<SuccessResponseDto<bool>>
    {
        public Guid Id { get; }

        public DeleteBillingPlanCommand(Guid id)
        {
            Id = id;
        }
    }
}