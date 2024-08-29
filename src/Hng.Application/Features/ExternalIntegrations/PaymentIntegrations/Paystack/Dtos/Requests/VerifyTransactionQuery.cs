using CSharpFunctionalExtensions;
using MediatR;

namespace Hng.Application.Features.ExternalIntegrations.PaymentIntegrations.Paystack.Dtos.Requests
{
    public record VerifyTransactionQuery(string Reference) : IRequest<Result<string>>;
}