using CSharpFunctionalExtensions;
using Hng.Application.Features.ExternalIntegrations.PaymentIntegrations.Paystack.Dtos.Responses;
using MediatR;

namespace Hng.Application.Features.ExternalIntegrations.PaymentIntegrations.Paystack.Dtos.Requests
{
    public class InitiateSubscriptionTransactionCommand : IRequest<Result<InitiateSubscriptionTransactionResponse>>
    {
        public string Email { get; set; }
        public decimal Amount { get; set; }
        public string Plan { get; set; }
        public string Frequency { get; set; }
    }

    public record SubscriptionInitialized(Guid SubId, string Type = nameof(SubscriptionInitialized));
}
