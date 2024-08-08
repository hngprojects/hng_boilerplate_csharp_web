using CSharpFunctionalExtensions;
using Hng.Application.Features.ExternalIntegrations.PaymentIntegrations.Paystack.Dtos.Requests;
using Hng.Application.Features.ExternalIntegrations.PaymentIntegrations.Paystack.Dtos.Responses;

namespace Hng.Application.Features.ExternalIntegrations.PaymentIntegrations.Paystack.Services
{
    public interface IPaystackClient
    {
        Task<Result<VerifyTransactionResponse>> VerifyTransaction(VerifyTransactionRequest request);
        Task<Result<VerifyTransferResponse>> VerifyTransfer(VerifyTransactionRequest request);
        Task<Result<InitializeTransactionResponse>> InitializeTransaction(InitializeTransactionRequest request);
    }
}