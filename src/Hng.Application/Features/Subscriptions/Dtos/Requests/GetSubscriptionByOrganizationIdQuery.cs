using Hng.Application.Features.Subscriptions.Dtos.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hng.Application.Features.Subscriptions.Dtos.Requests
{
    public class GetSubscriptionByOrganizationIdQuery : IRequest<GetSubscriptionByOrganizationIdResponse>
    {
        public Guid OrganizationId { get; set; }

        public GetSubscriptionByOrganizationIdQuery(Guid organizationId)
        {
            OrganizationId = organizationId;
        }

    }
}
