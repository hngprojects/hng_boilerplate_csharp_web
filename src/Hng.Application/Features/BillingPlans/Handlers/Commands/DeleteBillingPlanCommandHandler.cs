using Hng.Application.Features.BillingPlans.Commands;
using Hng.Application.Shared.Dtos;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using MediatR;

namespace Hng.Application.Features.BillingPlans.Handlers.Commands
{
    public class DeleteBillingPlanCommandHandler : IRequestHandler<DeleteBillingPlanCommand, SuccessResponseDto<bool>>
    {
        private readonly IRepository<BillingPlan> _repository;

        public DeleteBillingPlanCommandHandler(IRepository<BillingPlan> repository)
        {
            _repository = repository;
        }

        public async Task<SuccessResponseDto<bool>> Handle(DeleteBillingPlanCommand request, CancellationToken cancellationToken)
        {
            var billingPlan = await _repository.GetAsync(request.Id);
            if (billingPlan == null)
            {
                return new SuccessResponseDto<bool>
                {
                    Data = false,
                    Message = "Billing plan not found."
                };
            }

            await _repository.DeleteAsync(billingPlan);
            await _repository.SaveChanges();

            return new SuccessResponseDto<bool>
            {
                Data = true,
                Message = "Billing plan deleted successfully."
            };
        }
    }
}