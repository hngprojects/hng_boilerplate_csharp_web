using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hng.Application.Features.Subscriptions.Dtos.Responses
{
    public class GetSubscriptionByUserIdResponse
    {
        public bool Status { get; set; }
        public string Error { get; set; }
        //public Subscription Subscription { get; set; }
        public List<SubscriptionDto> Subscriptions { get; set; }
    }
}
