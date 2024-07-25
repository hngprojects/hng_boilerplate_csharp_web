using System.Text.Json.Serialization;

namespace Hng.Application.Features.PaymentIntegrations.Paystack.Dtos.Requests
{
    public abstract record PaymentRequestBase
    {
        /// <summary>
		/// Required - The current business' SECRET KEY
		/// </summary>
		[JsonIgnore]
        public string BusinessAuthorizationToken { get; set; }
    }
}
