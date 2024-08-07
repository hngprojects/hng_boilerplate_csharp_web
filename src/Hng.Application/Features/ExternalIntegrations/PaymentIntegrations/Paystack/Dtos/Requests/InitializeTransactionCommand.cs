using CSharpFunctionalExtensions;
using Hng.Application.Features.ExternalIntegrations.PaymentIntegrations.Paystack.Dtos.Responses;
using MediatR;

namespace Hng.Application.Features.ExternalIntegrations.PaymentIntegrations.Paystack.Dtos.Requests
{
    public record InitializeTransactionCommand : IRequest<Result<InitializeTransactionResponse>>
    {
        public string Email { get; set; }
        public decimal Amount { get; set; }
        public Guid ProductId { get; set; }
    }
}

public record ProductInitialized(Guid ProductId, string Type = nameof(ProductInitialized));