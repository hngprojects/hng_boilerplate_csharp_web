using CSharpFunctionalExtensions;
using Hng.Application.Features.PaymentIntegrations.Paystack.Dtos.Requests;
using Hng.Application.Features.PaymentIntegrations.Paystack.Dtos.Responses;
using Hng.Application.Features.PaymentIntegrations.Paystack.Handlers.Commands;
using Hng.Application.Features.PaymentIntegrations.Paystack.Handlers.Queries;
using Hng.Application.Features.PaymentIntegrations.Paystack.Services;
using Hng.Domain.Enums;
using Hng.Infrastructure.Repository.Interface;
using Hng.Infrastructure.Utilities.StringKeys;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Hng.Application.Test.Features.PaymentIntegrations.Paystack
{
    public class VerifyTransactionQueryShould
    {
        private readonly Mock<IPaystackClient> _repositoryMock;
        private readonly Mock<PaystackApiKeys> _repositoryMockApiKeys;
        private readonly VerifyTransactionQueryHandler _handler;

        public VerifyTransactionQueryShould()
        {
            _repositoryMock = new Mock<IPaystackClient>();
            _repositoryMockApiKeys = new Mock<PaystackApiKeys>();
            _handler = new VerifyTransactionQueryHandler(_repositoryMock.Object, _repositoryMockApiKeys.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailureOnNullReference()
        {
            var request = new VerifyTransactionQuery("");
            
            var result = await _handler.Handle(request, default);

            Assert.True(result.IsFailure);
            Assert.NotNull(result.Error);
            Assert.Equal(result.Error, "Reference cannot be null!");
        }

        //[Fact]
        //public async Task Handle_ShouldReturnSuccessOnValidReference()
        //{
        //    var request = new VerifyTransactionQuery("frsgtgfrg");
        //    var verifyRequest = new VerifyTransactionRequest(request.Reference) { BusinessAuthorizationToken = "gfrtshdjuenmmndda" };
        //    var response = new VerifyTransactionResponse() { status = true, message = "success", 
        //    data = new Application.Features.PaymentIntegrations.Paystack.Dtos.Responses.Data() 
        //    { 
        //        status = PaystackResponseStatus.success.ToString()
        //    }};

        //    _repositoryMock.Setup(r => r.VerifyTransaction(verifyRequest))
        //        .ReturnsAsync(Result.Success(response));

        //    var result = await _handler.Handle(request, default);

        //    Assert.True(result.IsSuccess);
        //    Assert.NotNull(result.Value);
        //    Assert.Equal(result.Value, "Success");
        //}
    }
}
