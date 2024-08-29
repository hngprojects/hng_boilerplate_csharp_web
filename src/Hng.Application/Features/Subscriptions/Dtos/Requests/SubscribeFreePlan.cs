using CSharpFunctionalExtensions;
using Hng.Application.Features.Subscriptions.Dtos.Responses;
using MediatR;

namespace Hng.Application.Features.Subscriptions.Dtos.Requests
{
    public record SubscribeFreePlan : IRequest<Result<SubscribeFreePlanResponse>>
    {
        public string UserId { get; set; }

        public string OrganizationId { get; set; }
    }
}
