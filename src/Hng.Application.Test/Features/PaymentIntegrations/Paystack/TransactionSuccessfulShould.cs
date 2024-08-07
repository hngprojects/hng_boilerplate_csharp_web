using Hng.Application.Features.ExternalIntegrations.PaymentIntegrations.Paystack.Dtos.Requests;
using Hng.Application.Features.ExternalIntegrations.PaymentIntegrations.Paystack.Handlers.Commands;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using Hng.Infrastructure.Utilities.StringKeys;
using Moq;
using Newtonsoft.Json;
using System.Linq.Expressions;
using Xunit;

namespace Hng.Application.Test.Features.PaymentIntegrations.Paystack
{
    public class TransactionSuccessfulShould
    {
        private readonly Mock<IRepository<Transaction>> _repositoryMock;
        private readonly ProductsTransactionCommandHandler _handler;

        public TransactionSuccessfulShould()
        {
            _repositoryMock = new Mock<IRepository<Transaction>>();
            _handler = new ProductsTransactionCommandHandler(_repositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnSuccessOnSuccessEvent()
        {
            var request = new TransactionsWebhookCommand()
            {
                Event = PaystackEventKeys.charge_success,
                Data = new()
                {
                    Message = JsonConvert.SerializeObject(new ProductInitialized(Guid.NewGuid()))
                }
            };
            var response = new Transaction() { Status = Domain.Enums.TransactionStatus.Completed };
            var transactionWebhookCommand = new TransactionWebhookCommand(request);

            _repositoryMock.Setup(r => r.GetBySpec(It.IsAny<Expression<Func<Transaction, bool>>>(), It.IsAny<Expression<Func<Transaction, object>>[]>()))
                .ReturnsAsync(response);

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
            var transactionWebhookCommand = new TransactionWebhookCommand(request);
            var result = await _handler.Handle(transactionWebhookCommand, default);

            Assert.True(!result);
        }
    }
}