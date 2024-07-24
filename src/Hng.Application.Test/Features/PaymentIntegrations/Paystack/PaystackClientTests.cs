using FluentAssertions;
using Hng.Application.Features.PaymentIntegrations.Paystack.Dtos.Requests;
using Hng.Application.Features.PaymentIntegrations.Paystack.Dtos.Responses;
using Hng.Application.Features.PaymentIntegrations.Paystack.Services;
using System.Net;
using Xunit;

namespace Hng.Application.Test.Features.PaymentIntegrations.Paystack
{
    public class PaystackClientTests
    {
        [Fact]
        public async Task VerifyTransactionRecipientResponse_Handles_SuccessResponse_AsExpected()
        {
            //Arrange
            var request = new VerifyTransactionRequest("trgfdEdr5");
            var response = new VerifyTransactionResponse() { };
            var authorizationToken = "2ODkPWxr61ZD04XZaXagDn1N0dArQraE00w9UrQK";
            var messageHandler = new FakeHttpMessageHandler<VerifyTransactionResponse>(response, HttpStatusCode.OK);
            var httpClient = new HttpClient(messageHandler);
            httpClient.BaseAddress = new("https://api.paystack.com");
            request.BusinessAuthorizationToken = authorizationToken;

            //Act
            var result = await new PaystackClient(httpClient).VerifyTransaction(request);

            //Assert
            result.Value.Should().BeOfType<VerifyTransactionResponse>();
            messageHandler.NumberOfCalls.Should().Be(1);
            messageHandler.Url.Should().Be("https://api.paystack.com/transaction/verify/trgfdEdr5");
            messageHandler.BusinessAuthorizationToken.Should().Be($"Bearer {authorizationToken}");
        }
    }
}