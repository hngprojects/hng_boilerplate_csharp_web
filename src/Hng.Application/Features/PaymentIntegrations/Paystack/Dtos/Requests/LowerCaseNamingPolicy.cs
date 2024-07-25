using System.Text.Json;

namespace Hng.Application.Features.PaymentIntegrations.Paystack.Dtos.Requests
{
    public class LowerCaseNamingPolicy : JsonNamingPolicy
    {
        public override string ConvertName(string name) => name.ToLower();
    }
}
