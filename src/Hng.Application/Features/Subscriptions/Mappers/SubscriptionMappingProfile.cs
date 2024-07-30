using Hng.Application.Features.Organisations.Dtos;
using Hng.Application.Features.Subscriptions.Dtos.Responses;
using Hng.Domain.Entities;

namespace Hng.Application.Features.Subscriptions.Mappers
{
    public class SubscriptionMappingProfile : AutoMapper.Profile
    {
        public SubscriptionMappingProfile()
        {
            CreateMap<Subscription, SubscriptionDto>();
            CreateMap<Organization, SubscriptionDto>();
        }
    }
}