namespace Hng.Application.Features.PaymentIntegrations.Paystack.Dtos.Requests
{
    public record InitializeTransactionRequest : PaymentRequestBase
    {
        public string Email { get; set; }
        public string Amount { get; set; }
        public string Reference { get; set; }
        public string CallbackUrl { get; set; }
        public string Metadata { get; set; }
        public Guid UserId { get; set; }
        public Guid ProductId { get; set; }

        public InitializeTransactionRequest(string amount, string email)
        {
            Amount = amount;
            Email = email;
        }
    }
}