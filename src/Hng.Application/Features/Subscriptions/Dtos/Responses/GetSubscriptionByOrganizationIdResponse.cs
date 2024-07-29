using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hng.Application.Features.Subscriptions.Dtos.Responses
{
    public class GetSubscriptionByOrganizationIdResponse
    {
        public bool Status { get; set; }
        public string Error { get; set; }
        public List<SubscriptionDto> Subscriptions { get; set; }
    }
}
