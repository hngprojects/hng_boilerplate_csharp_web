using AutoMapper;
using Hng.Application.Features.BillingPlans.Commands;
using Hng.Application.Features.BillingPlans.Dtos;
using Hng.Application.Shared.Dtos;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using MediatR;

namespace Hng.Application.Features.BillingPlans.Handlers.Commands
{
    public class CreateBillingPlanCommandHandler : IRequestHandler<CreateBillingPlanCommand, SuccessResponseDto<BillingPlanDto>>
    {
        private readonly IRepository<BillingPlan> _repository;
        private readonly IMapper _mapper;

        public CreateBillingPlanCommandHandler(IRepository<BillingPlan> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<SuccessResponseDto<BillingPlanDto>> Handle(CreateBillingPlanCommand request, CancellationToken cancellationToken)
        {
            var billingPlan = _mapper.Map<BillingPlan>(request.BillingPlanDto);
            billingPlan.CreatedAt = DateTime.UtcNow;

            var createdBillingPlan = await _repository.AddAsync(billingPlan);
            if (createdBillingPlan == null)
            {
                return new SuccessResponseDto<BillingPlanDto>
                {
                    Data = null,
                    Message = "Failed to create billing plan."
                };
            }

            await _repository.SaveChanges();
            return new SuccessResponseDto<BillingPlanDto>
            {
                Data = _mapper.Map<BillingPlanDto>(createdBillingPlan),
                Message = "Billing plan created successfully."
            };
        }
    }
}