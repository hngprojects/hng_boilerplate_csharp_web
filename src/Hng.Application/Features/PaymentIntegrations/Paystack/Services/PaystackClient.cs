using CSharpFunctionalExtensions;
using Hng.Application.Features.PaymentIntegrations.Paystack.Dtos.Requests;
using Hng.Application.Features.PaymentIntegrations.Paystack.Dtos.Responses;
using Newtonsoft.Json;
using System.Text;
using System.Text.Json;

namespace Hng.Application.Features.PaymentIntegrations.Paystack.Services
{
    public class PaystackClient : IPaystackClient
    {
        private const string Authorization = nameof(Authorization);
        private const string Bearer = nameof(Bearer);
        private const string MediaType = "application/json";
        private readonly HttpClient _client;

        private readonly JsonSerializerOptions SerializerOptions = new()
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = new LowerCaseNamingPolicy()
        };

        private static class Endpoints
        {
            public static string VerifyTransfer => "transfer/verify/{0}";
            public static string TransactionInitialize => "transaction/initialize";
            public static string VerifyTransaction => "transaction/verify/{0}";
        }

        public PaystackClient(HttpClient client)
        {
            _client = client;
        }

        public async Task<Result<VerifyTransferResponse>> VerifyTransfer(VerifyTransactionRequest request)
            => await FetchFromPaystack<VerifyTransferResponse, VerifyTransactionRequest>(request, Endpoints.VerifyTransfer);

        public async Task<Result<VerifyTransactionResponse>> VerifyTransaction(VerifyTransactionRequest request)
            => await FetchFromPaystack<VerifyTransactionResponse, VerifyTransactionRequest>(request, Endpoints.VerifyTransaction);

        public async Task<Result<InitializeTransactionResponse>> InitializeTransaction(InitializeTransactionRequest request)
            => await SendToPaystack<InitializeTransactionResponse, InitializeTransactionRequest>(request, Endpoints.TransactionInitialize);

        private async Task<Result<U>> SendToPaystack<U, T>(T request, string endpoint) where T : PaymentRequestBase
        {
            ArgumentNullException.ThrowIfNull(request.BusinessAuthorizationToken);
            var authorizedClient = SetAuthToken(_client, request.BusinessAuthorizationToken);
            try
            {
                var serializedRequest = System.Text.Json.JsonSerializer.Serialize(request, SerializerOptions);
                var body = new StringContent(serializedRequest, Encoding.UTF8, MediaType);
                var httpResponse = await authorizedClient.PostAsync(endpoint, body);

                if (!httpResponse.IsSuccessStatusCode)
                    return Result.Failure<U>(await httpResponse.Content.ReadAsStringAsync());

                var response =
                    JsonConvert.DeserializeObject<U>(await httpResponse.Content.ReadAsStringAsync());
                return Result.Success(response);
            }
            catch (Exception ex)
            {
                return Result.Failure<U>(ex.Message);
            }
        }

        private async Task<Result<U>> FetchFromPaystack<U, T>(T requestParam, string endpoint)
            where T : PaymentQueryBase<string>
        {
            var authorizedClient = SetAuthToken(_client, requestParam.BusinessAuthorizationToken);
            try
            {
                var requestPath = string.Format(endpoint, requestParam.Param);
                var httpResponse = await authorizedClient.GetAsync(requestPath);

                if (!httpResponse.IsSuccessStatusCode)
                    return Result.Failure<U>(await httpResponse.Content.ReadAsStringAsync());
                var response =
                    System.Text.Json.JsonSerializer.Deserialize<U>(await httpResponse.Content.ReadAsStringAsync(), SerializerOptions);
                return Result.Success(response);
            }
            catch (Exception ex)
            {
                return Result.Failure<U>(ex.Message);
            }
        }

        private static HttpClient SetAuthToken(HttpClient client, string businessAuthorizationToken)
        {
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add(Authorization,
                string.Format("{0} {1}", Bearer, businessAuthorizationToken));
            return client;
        }
    }
}