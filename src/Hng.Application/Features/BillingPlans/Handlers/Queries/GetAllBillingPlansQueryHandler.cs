using AutoMapper;
using Hng.Application.Features.BillingPlans.Dtos;
using Hng.Application.Features.BillingPlans.Queries;
using Hng.Application.Shared.Dtos;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using MediatR;

namespace Hng.Application.Features.BillingPlans.Handlers.Queries
{
    public class GetAllBillingPlansQueryHandler : IRequestHandler<GetAllBillingPlansQuery, PaginatedResponseDto<List<BillingPlanDto>>>
    {
        private readonly IRepository<BillingPlan> _repository;
        private readonly IMapper _mapper;

        public GetAllBillingPlansQueryHandler(IRepository<BillingPlan> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<PaginatedResponseDto<List<BillingPlanDto>>> Handle(GetAllBillingPlansQuery request, CancellationToken cancellationToken)
        {
            var totalCount = await _repository.CountAsync();
            var billingPlans = await _repository.GetAllAsync();
            var billingPlanDtos = _mapper.Map<List<BillingPlanDto>>(billingPlans);

            var paginatedBillingPlans = billingPlanDtos
                .Skip((request.QueryParameters.Offset - 1) * request.QueryParameters.Limit)
                .Take(request.QueryParameters.Limit)
                .ToList();

            var metadata = new PagedListMetadataDto
            {
                TotalCount = totalCount,
                PageSize = request.QueryParameters.Limit,
                CurrentPage = request.QueryParameters.Offset,
                TotalPages = (int)Math.Ceiling(totalCount / (double)request.QueryParameters.Limit)
            };

            return new PaginatedResponseDto<List<BillingPlanDto>>
            {
                Data = paginatedBillingPlans,
                Metadata = metadata
            };
        }
    }
}