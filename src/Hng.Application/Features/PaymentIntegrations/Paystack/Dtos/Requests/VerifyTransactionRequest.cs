using JasperFx.Core;

namespace Hng.Application.Features.PaymentIntegrations.Paystack.Dtos.Requests
{
    public record VerifyTransactionRequest : PaymentQueryBase<string>
    {
        public VerifyTransactionRequest(string transactionReference)
        {
            if (transactionReference.IsEmpty())
                throw new ArgumentNullException(nameof(transactionReference));

            Param = transactionReference;
        }
    }
}