using AutoMapper;
using Hng.Application.Features.BillingPlans.Dtos;
using Hng.Application.Features.BillingPlans.Queries;
using Hng.Application.Shared.Dtos;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using MediatR;

namespace Hng.Application.Features.BillingPlans.Handlers.Queries
{
    public class GetBillingPlanByIdQueryHandler : IRequestHandler<GetBillingPlanByIdQuery, SuccessResponseDto<BillingPlanDto>>
    {
        private readonly IRepository<BillingPlan> _repository;
        private readonly IMapper _mapper;

        public GetBillingPlanByIdQueryHandler(IRepository<BillingPlan> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<SuccessResponseDto<BillingPlanDto>> Handle(GetBillingPlanByIdQuery request, CancellationToken cancellationToken)
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

            return new SuccessResponseDto<BillingPlanDto>
            {
                Data = _mapper.Map<BillingPlanDto>(billingPlan),
                Message = "Billing plan retrieved successfully."
            };
        }
    }
}