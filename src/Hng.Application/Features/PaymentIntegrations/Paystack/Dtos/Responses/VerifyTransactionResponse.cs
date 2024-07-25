namespace Hng.Application.Features.PaymentIntegrations.Paystack.Dtos.Responses
{
    public record VerifyTransactionResponse
    {
        public bool status { get; set; }
        public string message { get; set; }
        public Data data { get; set; }
    }

    public record Data
    {
        public long id { get; set; }
        public string domain { get; set; }
        public string status { get; set; }
        public string reference { get; set; }
        public object receipt_number { get; set; }
        public int amount { get; set; }
        public object message { get; set; }
        public string gateway_response { get; set; }
        public DateTime paid_at { get; set; }
        public DateTime created_at { get; set; }
        public string channel { get; set; }
        public string currency { get; set; }
        public string ip_address { get; set; }
        public Metadata metadata { get; set; }
        public Log log { get; set; }
        public int fees { get; set; }
        public object fees_split { get; set; }
        public Authorization authorization { get; set; }
        public Customer customer { get; set; }
        public object plan { get; set; }
        public Split split { get; set; }
        public object order_id { get; set; }
        public DateTime paidAt { get; set; }
        public DateTime createdAt { get; set; }
        public int requested_amount { get; set; }
        public object pos_transaction_data { get; set; }
        public object source { get; set; }
        public object fees_breakdown { get; set; }
        public object connect { get; set; }
        public DateTime transaction_date { get; set; }
        public Plan_Object plan_object { get; set; }
        public Subaccount subaccount { get; set; }
    }

    public record Metadata
    {
        public string referrer { get; set; }
    }

    public record Log
    {
        public int start_time { get; set; }
        public int time_spent { get; set; }
        public int attempts { get; set; }
        public int errors { get; set; }
        public bool success { get; set; }
        public bool mobile { get; set; }
        public object[] input { get; set; }
        public History[] history { get; set; }
    }

    public record History
    {
        public string type { get; set; }
        public string message { get; set; }
        public int time { get; set; }
    }

    public record Authorization
    {
        public string authorization_code { get; set; }
        public string bin { get; set; }
        public string last4 { get; set; }
        public string exp_month { get; set; }
        public string exp_year { get; set; }
        public string channel { get; set; }
        public string card_type { get; set; }
        public string bank { get; set; }
        public string country_code { get; set; }
        public string brand { get; set; }
        public bool reusable { get; set; }
        public string signature { get; set; }
        public string account_name { get; set; }
        public string receiver_bank_account_number { get; set; }
        public string receiver_bank { get; set; }
    }

    public record Customer
    {
        public long id { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string email { get; set; }
        public string customer_code { get; set; }
        public string phone { get; set; }
        public object metadata { get; set; }
        public string risk_action { get; set; }
        public string international_format_phone { get; set; }
    }

    public record Split
    {
    }

    public record Plan_Object
    {
    }

    public record Subaccount
    {
    }
}
