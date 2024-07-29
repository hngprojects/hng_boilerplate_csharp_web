using Hng.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hng.Application.Features.Subscriptions.Mappers
{
    public class SubscriptionMappingProfile : AutoMapper.Profile
    {
        public SubscriptionMappingProfile()
        {
            CreateMap<Subscription, GetSubscriptionByUserIdResponse>().ReverseMap();
            CreateMap<Subscription, GetSubscriptionByOrganizationIdResponse>();
        }
    }
}
