using Hng.Application.Features.Subscriptions.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hng.Application.Features.Subscriptions.Commands
{
    public class ActivateSubscriptionCommand : IRequest<SubscriptionDto>
    {
        public ActivateSubscriptionCommand(Guid subscriptionId)
        {
            SubscriptionId = subscriptionId;
        }

        public Guid SubscriptionId { get; }
    }
}
