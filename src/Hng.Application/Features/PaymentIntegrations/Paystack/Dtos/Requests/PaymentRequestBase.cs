using System.Text.Json.Serialization;

namespace Hng.Application.Features.PaymentIntegrations.Paystack.Dtos.Requests
{
    public abstract record PaymentRequestBase
    {
        /// <summary>
        /// The current business' SECRET KEY
        /// </summary>
		[JsonIgnore]
        public string BusinessAuthorizationToken { get; set; }
    }
}