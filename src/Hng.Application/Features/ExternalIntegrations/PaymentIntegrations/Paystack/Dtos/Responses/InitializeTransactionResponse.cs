namespace Hng.Application.Features.ExternalIntegrations.PaymentIntegrations.Paystack.Dtos.Responses
{
    public class InitializeTransactionResponse
    {
        public bool Status { get; set; }
        public string Message { get; set; }
        public InitializeTransactionData Data { get; set; }
    }

    public class InitializeTransactionData
    {
        public string authorization_url { get; set; }
        public string access_code { get; set; }
        public string Reference { get; set; }
    }
}
