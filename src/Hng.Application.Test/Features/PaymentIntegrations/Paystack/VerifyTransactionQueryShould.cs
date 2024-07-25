using Hng.Application.Features.PaymentIntegrations.Paystack.Dtos.Requests;
using Hng.Application.Features.PaymentIntegrations.Paystack.Handlers.Queries;
using Hng.Application.Features.PaymentIntegrations.Paystack.Services;
using Hng.Infrastructure.Utilities.StringKeys;
using Moq;
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
            Assert.Equal("Reference cannot be null!", result.Error);
        }
    }
}