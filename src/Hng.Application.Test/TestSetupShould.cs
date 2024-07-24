using AutoMapper;
using CSharpFunctionalExtensions;
using Hng.Application.Features.PaymentIntegrations.Paystack.Dtos.Requests;
using Hng.Application.Features.PaymentIntegrations.Paystack.Dtos.Responses;
using Hng.Application.Features.PaymentIntegrations.Paystack.Handlers.Commands;
using Hng.Application.Features.PaymentIntegrations.Paystack.Services;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using Hng.Infrastructure.Utilities.StringKeys;
using Moq;
using Xunit;

namespace Hng.Application.Test;

public class TestSetupShould
{

    public class InitializeTransactionCommandHandlerTests
    {
        private readonly Mock<IPaystackClient> _paystackClientMock;
        private readonly Mock<PaystackApiKeys> _apiKeysMock;
        private readonly Mock<IRepository<Transaction>> _paymentRepoMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly InitializeTransactionCommandHandler _handler;

        public InitializeTransactionCommandHandlerTests()
        {
            _paystackClientMock = new Mock<IPaystackClient>();
            _apiKeysMock = new Mock<PaystackApiKeys>();
            _paymentRepoMock = new Mock<IRepository<Transaction>>();
            _mapperMock = new Mock<IMapper>();
            _handler = new InitializeTransactionCommandHandler(_paystackClientMock.Object, _apiKeysMock.Object, _paymentRepoMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnSuccess_WhenTransactionIsInitialized()
        {
            // Arrange
            var command = new InitializeTransactionCommand
            {
                Email = "customer@example.com",
                Amount = 50000,
                ProductId = Guid.NewGuid()
            };

            var initializeResponse = new InitializeTransactionResponse
            {
                Status = true,
                Message = "Transaction initialized",
                Data = new InitializeTransactionData
                {
                    authorization_url = "https://checkout.paystack.com/v9hjqej3vn178o2\",",
                    access_code = "v9hjqej3vn178o2",
                    Reference = "hng638574416963812624"
                }
            };

            _paystackClientMock
                .Setup(x => x.InitializeTransaction(It.IsAny<InitializeTransactionRequest>()))
                .ReturnsAsync(Result.Success(initializeResponse));

            _mapperMock
            .Setup(m => m.Map<Transaction>(It.IsAny<InitializeTransactionCommand>()))
            .Returns(new Transaction());

            // Set up _paymentRepo mock
            _paymentRepoMock
                .Setup(r => r.AddAsync(It.IsAny<Transaction>()))
                   .ReturnsAsync(new Transaction());

            _paymentRepoMock
                .Setup(r => r.SaveChanges())
                .Returns(Task.CompletedTask);

            // Act
            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(initializeResponse.Status, result.Value.Status);
            Assert.Equal(initializeResponse.Message, result.Value.Message);
            Assert.Equal(initializeResponse.Data.authorization_url, result.Value.Data.authorization_url);
            Assert.Equal(initializeResponse.Data.access_code, result.Value.Data.access_code);
            Assert.Equal(initializeResponse.Data.Reference, result.Value.Data.Reference);

            // Verify that AddAsync and SaveChanges were called
            _paymentRepoMock.Verify(r => r.AddAsync(It.IsAny<Transaction>()), Times.Once);
            _paymentRepoMock.Verify(r => r.SaveChanges(), Times.Once);

        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenTransactionFails()
        {
            // Arrange
            var command = new InitializeTransactionCommand
            {
                Email = "customer@example.com",
                Amount = 50000,
            };

            _paystackClientMock
                .Setup(x => x.InitializeTransaction(It.IsAny<InitializeTransactionRequest>()))
                .ReturnsAsync(Result.Failure<InitializeTransactionResponse>("An unexpected error occurred. Please try again later or contact support."));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("An unexpected error occurred. Please try again later or contact support.", result.Error);
        }

    }
}
