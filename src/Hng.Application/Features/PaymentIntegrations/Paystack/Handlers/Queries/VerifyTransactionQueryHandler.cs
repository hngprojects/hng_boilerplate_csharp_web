using CSharpFunctionalExtensions;
using Hng.Application.Features.PaymentIntegrations.Paystack.Dtos.Requests;
using Hng.Application.Features.PaymentIntegrations.Paystack.Services;
using Hng.Domain.Enums;
using Hng.Infrastructure.Utilities.StringKeys;
using MediatR;

namespace Hng.Application.Features.PaymentIntegrations.Paystack.Handlers.Queries
{
    public class VerifyTransactionQueryHandler : IRequestHandler<VerifyTransactionQuery, Result<string>>
    {
        private readonly IPaystackClient _paystackClient;
        private readonly PaystackApiKeys _apiKey;

        public VerifyTransactionQueryHandler(
            IPaystackClient paystackClient,
            PaystackApiKeys apiKey)
        {
            _paystackClient = paystackClient;
            _apiKey = apiKey;
        }

        public async Task<Result<string>> Handle(VerifyTransactionQuery request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Reference))
                return Result.Failure<string>("Reference cannot be null!");

            var verifyRequest = new VerifyTransactionRequest(request.Reference) { BusinessAuthorizationToken = _apiKey.SecretKey };

            var verifyResponse = await _paystackClient.VerifyTransfer(verifyRequest);

            if (verifyResponse.IsSuccess && verifyResponse.Value.Status
                    && verifyResponse.Value.Data.Status == PaystackResponseStatus.success.ToString())
            {
                return Result.Success("Success");
            }

            return Result.Failure<string>("Verification Failed");
        }
    }
}
