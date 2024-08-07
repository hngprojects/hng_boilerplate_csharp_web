namespace Hng.Application.Features.ExternalIntegrations.PaymentIntegrations.Paystack.Dtos.Requests
{
    public record InitializeTransactionRequest : PaymentRequestBase
    {
        public string Email { get; set; }
        public string Amount { get; set; }
        public string Reference { get; set; }
        public string CallbackUrl { get; set; }
        public string Metadata { get; set; }

        public InitializeTransactionRequest(string amount, string email, string refernce)
        {
            Amount = amount;
            Email = email;
            Reference = refernce;
        }
    }
}