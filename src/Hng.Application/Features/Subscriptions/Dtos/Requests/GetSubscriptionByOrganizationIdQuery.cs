using Hng.Application.Features.Organisations.Dtos;
using Hng.Application.Features.Subscriptions.Dtos.Responses;
using MediatR;


namespace Hng.Application.Features.Subscriptions.Dtos.Requests
{
    public class GetSubscriptionByOrganizationIdQuery : IRequest<SubscriptionDto>
    {
        public Guid OrganizationId { get; set; }

        public GetSubscriptionByOrganizationIdQuery(Guid organizationId)
        {
            OrganizationId = organizationId;
        }

    }
}