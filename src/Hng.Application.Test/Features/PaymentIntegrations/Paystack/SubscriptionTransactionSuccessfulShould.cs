using Hng.Application.Features.PaymentIntegrations.Paystack.Dtos.Requests;
using Hng.Application.Features.PaymentIntegrations.Paystack.Handlers.Commands;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using Hng.Infrastructure.Utilities.StringKeys;
using Moq;
using Newtonsoft.Json;
using System.Linq.Expressions;
using Xunit;

namespace Hng.Application.Test.Features.PaymentIntegrations.Paystack
{
    public class SubscriptionTransactionSuccessfulShould
    {
        private readonly Mock<IRepository<Transaction>> _repositoryMock;
        private readonly Mock<IRepository<Subscription>> _subRepoMock;
        private readonly SubscriptionsTransactionCommandHandler _handler;

        public SubscriptionTransactionSuccessfulShould()
        {
            _repositoryMock = new Mock<IRepository<Transaction>>();
            _subRepoMock = new Mock<IRepository<Subscription>>();
            _handler = new SubscriptionsTransactionCommandHandler(_repositoryMock.Object, _subRepoMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnSuccessOnSuccessEvent()
        {
            var request = new TransactionsWebhookCommand()
            {
                Event = PaystackEventKeys.charge_success,
                Data = new()
                {
                    Message = JsonConvert.SerializeObject(new SubscriptionInitialized(Guid.NewGuid()))
                }
            };
            var response = new Transaction() { Status = Domain.Enums.TransactionStatus.Completed };
            var subResponse = new Subscription() { IsActive = true, StartDate = DateTime.UtcNow };
            var transactionWebhookCommand = new SubTransactionWebhookCommand(request);

            _repositoryMock.Setup(r => r.GetBySpec(It.IsAny<Expression<Func<Transaction, bool>>>()))
                .ReturnsAsync(response);
            _subRepoMock.Setup(r => r.GetBySpec(It.IsAny<Expression<Func<Subscription, bool>>>()))
                .ReturnsAsync(subResponse);

            var result = await _handler.Handle(transactionWebhookCommand, default);

            Assert.True(result);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailureOnWrongReferenceEvent()
        {
            var request = new TransactionsWebhookCommand()
            {
                Event = PaystackEventKeys.charge_success,
                Data = new()
                {
                    Message = JsonConvert.SerializeObject(new ProductInitialized(Guid.NewGuid()))
                }
            };
            var transactionWebhookCommand = new SubTransactionWebhookCommand(request);
            var result = await _handler.Handle(transactionWebhookCommand, default);

            Assert.True(!result);
        }
    }
}
