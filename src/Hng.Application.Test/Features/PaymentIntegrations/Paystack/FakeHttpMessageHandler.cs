using System.Net;
using System.Text;
using System.Text.Json;

namespace Hng.Application.Test.Features.PaymentIntegrations.Paystack
{
    public class FakeHttpMessageHandler<T> : HttpMessageHandler
    {
        public FakeHttpMessageHandler(T response, HttpStatusCode statusCode)
        {
            Response = response;
            StatusCode = statusCode;
        }

        public string Url { get; private set; }

        public int NumberOfCalls { get; private set; }

        public string BusinessAuthorizationToken { get; private set; }

        private T Response { get; }

        private HttpStatusCode StatusCode { get; }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            NumberOfCalls++;
            Url = request.RequestUri.ToString();
            request.Headers.TryGetValues("authorization", out var authorizationToken);
            if (authorizationToken is not null)
            {
                BusinessAuthorizationToken = authorizationToken.First();
            }

            return new()
            {
                StatusCode = StatusCode,
                Content = new StringContent(JsonSerializer.Serialize(Response), Encoding.UTF8, "application/json")
            };
        }
    }
}
