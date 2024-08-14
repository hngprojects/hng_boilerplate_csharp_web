using AutoMapper;
using Hng.Application.Features.BillingPlans.Commands;
using Hng.Application.Features.BillingPlans.Dtos;
using Hng.Application.Shared.Dtos;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using MediatR;

namespace Hng.Application.Features.BillingPlans.Handlers.Commands
{
    public class UpdateBillingPlanCommandHandler : IRequestHandler<UpdateBillingPlanCommand, SuccessResponseDto<BillingPlanDto>>
    {
        private readonly IRepository<BillingPlan> _repository;
        private readonly IMapper _mapper;

        public UpdateBillingPlanCommandHandler(IRepository<BillingPlan> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<SuccessResponseDto<BillingPlanDto>> Handle(UpdateBillingPlanCommand request, CancellationToken cancellationToken)
        {
            var billingPlan = await _repository.GetAsync(request.Id);
            if (billingPlan == null)
            {
                return new SuccessResponseDto<BillingPlanDto>
                {
                    Data = null,
                    Message = "Billing plan not found."
                };
            }

            _mapper.Map(request.BillingPlanDto, billingPlan);
            billingPlan.UpdatedAt = DateTime.UtcNow;
            await _repository.UpdateAsync(billingPlan);
            await _repository.SaveChanges();

            return new SuccessResponseDto<BillingPlanDto>
            {
                Data = _mapper.Map<BillingPlanDto>(billingPlan),
                Message = "Billing plan updated successfully."
            };
        }
    }
}