using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Hng.Domain.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum TransactionIntegrationPartners
    {
        Paystack,
        Stripe,
        Flutterwave
    }
}
