using System.Text.Json.Serialization;

namespace Hng.Application.Features.PaymentIntegrations.Paystack.Dtos.Requests
{
    public abstract record PaymentRequestBase
    {
		[JsonIgnore]
        public string BusinessAuthorizationToken { get; set; }
    }
}
