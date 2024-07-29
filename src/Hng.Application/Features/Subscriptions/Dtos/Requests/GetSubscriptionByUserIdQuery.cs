using Hng.Application.Features.Subscriptions.Dtos.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hng.Application.Features.Subscriptions.Dtos.Requests
{
    public class GetSubscriptionByUserIdQuery : IRequest<GetSubscriptionByUserIdResponse>
    {
        public Guid UserId { get; set; }

        public GetSubscriptionByUserIdQuery(Guid userId)
        {
            UserId = userId;
        }
    }
}
