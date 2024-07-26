using CSharpFunctionalExtensions;
using Hng.Application.Features.PaymentIntegrations.Paystack.Dtos.Responses;
using MediatR;


namespace Hng.Application.Features.PaymentIntegrations.Paystack.Dtos.Requests
{
    public record InitializeTransactionCommand : IRequest<Result<InitializeTransactionResponse>>
    {
        public string Email { get; set; }
        public decimal Amount { get; set; }
        public Guid ProductId { get; set; }
    }
}
