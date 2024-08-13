using Hng.Application.Features.BillingPlans.Dtos;
using Hng.Domain.Entities;

namespace Hng.Application.Features.BillingPlans.Mappers
{
    public class BillingPlanMapperProfile : AutoMapper.Profile
    {
        public BillingPlanMapperProfile()
        {
            CreateMap<BillingPlan, BillingPlanDto>().ReverseMap();
            CreateMap<CreateBillingPlanDto, BillingPlan>();
        }
    }
}