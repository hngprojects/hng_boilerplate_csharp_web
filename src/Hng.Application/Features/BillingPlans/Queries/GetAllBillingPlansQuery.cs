using Hng.Application.Features.BillingPlans.Dtos;
using Hng.Application.Shared.Dtos;
using MediatR;

namespace Hng.Application.Features.BillingPlans.Queries
{
    public class GetAllBillingPlansQuery : IRequest<PaginatedResponseDto<List<BillingPlanDto>>>
    {
        public BaseQueryParameters QueryParameters { get; }

        public GetAllBillingPlansQuery(BaseQueryParameters queryParameters)
        {
            QueryParameters = queryParameters;
        }
    }
}