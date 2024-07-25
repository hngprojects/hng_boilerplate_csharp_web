using System.Net;

namespace Hng.Application.Features.PaymentIntegrations.Paystack.Dtos.Responses
{
    public record VerifyTransferResponse
    {
        public bool Status { get; set; }

        public string Message { get; set; }

        public TransferData Data { get; set; }
    }

    public record TransferData
    {
        public int Integration { get; set; }
        public Recipient Recipient { get; set; }
        public string Domain { get; set; }
        public int Amount { get; set; }
        public string Currency { get; set; }
        public string Reference { get; set; }
        public string Source { get; set; }
        public string Source_Details { get; set; }
        public string Reason { get; set; }
        public Authorization Authorization { get; set; }
        public string Status { get; set; }
        public string Failures { get; set; }
        public string Transfer_Code { get; set; }
        public string Titan_Code { get; set; }
        public string Transferred_At { get; set; }
        public long Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public record Recipient
    {
        public string Domain { get; set; }
        public string Type { get; set; }
        public string Currency { get; set; }
        public string Name { get; set; }
        public Details Details { get; set; }
        public string Description { get; set; }
        public object Metadata { get; set; }
        public string Recipient_Code { get; set; }
        public bool Active { get; set; }
        public string Email { get; set; }
        public long Id { get; set; }
        public int Integration { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public record Details
    {
        public string Account_Number { get; set; }
        public string Account_Name { get; set; }
        public string Bank_Code { get; set; }
        public string Bank_Name { get; set; }
    }
}
