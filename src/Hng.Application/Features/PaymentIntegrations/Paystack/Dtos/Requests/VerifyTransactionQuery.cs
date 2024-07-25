using CSharpFunctionalExtensions;
using Hng.Application.Features.UserManagement.Dtos;
using MediatR;

namespace Hng.Application.Features.PaymentIntegrations.Paystack.Dtos.Requests
{
    public record VerifyTransactionQuery(string Reference) : IRequest<Result<string>>;
}
