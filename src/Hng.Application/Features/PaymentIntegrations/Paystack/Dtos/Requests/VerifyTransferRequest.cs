using Hng.Application.Features.PaymentIntegrations.Paystack.Dtos.Common;
using JasperFx.Core;

namespace Hng.Application.Features.PaymentIntegrations.Paystack.Dtos.Requests
{
    public record VerifyTransferRequest : PaymentQueryBase<string>
    {
        public VerifyTransferRequest(string transactionReference)
        {
            if (transactionReference.IsEmpty())
                throw new ArgumentNullException(nameof(transactionReference));

            Param = transactionReference;
        }
    }
}
