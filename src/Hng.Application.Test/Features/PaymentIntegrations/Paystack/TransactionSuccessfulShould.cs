using Hng.Application.Features.PaymentIntegrations.Paystack.Dtos.Requests;
using Hng.Application.Features.PaymentIntegrations.Paystack.Handlers.Commands;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using Hng.Infrastructure.Utilities.StringKeys;
using Moq;
using System.Linq.Expressions;
using Xunit;

namespace Hng.Application.Test.Features.PaymentIntegrations.Paystack
{
    public class TransactionSuccessfulShould
    {
        private readonly Mock<IRepository<Transaction>> _repositoryMock;
        private readonly TransactionSuccessfulCommandHandler _handler;

        public TransactionSuccessfulShould()
        {
            _repositoryMock = new Mock<IRepository<Transaction>>();
            _handler = new TransactionSuccessfulCommandHandler(_repositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnSuccessOnSuccessEvent()
        {
            var request = new TransactionSuccessfulCommand() { Event = PaystackEventKeys.charge_success };
            var response = new Transaction() { Status = Domain.Enums.TransactionStatus.Completed };

            _repositoryMock.Setup(r => r.GetBySpec(It.IsAny<Expression<Func<Transaction, bool>>>(), It.IsAny<Expression<Func<Transaction, object>>[]>()))
                .ReturnsAsync(response);

            var result = await _handler.Handle(request, default);

            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal("success", result.Value);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailureOnWrongReferenceEvent()
        {
            var request = new TransactionSuccessfulCommand() { Event = PaystackEventKeys.charge_success };
            var result = await _handler.Handle(request, default);

            Assert.True(result.IsFailure);
            Assert.NotNull(result.Error);
            Assert.Equal("Transaction not found", result.Error);
        }
    }
}