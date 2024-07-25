using System.Text.Json.Serialization;

namespace Hng.Application.Features.PaymentIntegrations.Paystack.Dtos.Requests
{
    public record PaymentQueryBase<T>
    {
        public T Param { get; set; }

        /// <summary>
        /// Required - The current business' SECRET KEY
        /// </summary>
        [JsonIgnore]
        public string BusinessAuthorizationToken { get; set; }
    }
}
