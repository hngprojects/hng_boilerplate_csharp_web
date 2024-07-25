using CSharpFunctionalExtensions;
using MediatR;
using Newtonsoft.Json;

namespace Hng.Application.Features.PaymentIntegrations.Paystack.Dtos.Requests
{
    public record TransactionSuccessfulCommand : IRequest<Result<string>>
    {
        public string Event { get; set; }

        public Data Data { get; set; }
    }

    public record Data
    {
        public long Id { get; set; }
        public string Domain { get; set; }
        public string Status { get; set; }
        public string Reference { get; set; }
        public long Amount { get; set; }
        public object Message { get; set; }
        [JsonProperty("paid_at")]
        public string PaidAt { get; set; }
        public string CreatedAt { get; set; }
        public string Channel { get; set; }
        public string Currency { get; set; }
        public object Metadata { get; set; }
        public Authorization Authorization { get; set; }
    }

    public record Authorization
    {
        public string AuthorizationCode { get; set; }
        public string Bin { get; set; }
        public string Last4 { get; set; }
        public string ExpMonth { get; set; }
        public string ExpYear { get; set; }
        public string CardType { get; set; }
        public string Bank { get; set; }
        public string CountryCode { get; set; }
        public string Brand { get; set; }
        public string AccountName { get; set; }
    }
}
