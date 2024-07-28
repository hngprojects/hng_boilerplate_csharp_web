﻿using CSharpFunctionalExtensions;
using Hng.Application.Features.PaymentIntegrations.Paystack.Dtos.Requests;
using Hng.Application.Features.PaymentIntegrations.Paystack.Dtos.Responses;
using Hng.Application.Features.PaymentIntegrations.Paystack.Handlers.Commands;
using Hng.Application.Features.PaymentIntegrations.Paystack.Services;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using Hng.Infrastructure.Utilities.StringKeys;
using Moq;
using System.Linq.Expressions;
using Xunit;

namespace Hng.Application.Test.Features.PaymentIntegrations.Paystack
{
    public class InitializeTransaction
    {
        public class InitializeTransactionCommandHandlerTests
        {
            private readonly Mock<IPaystackClient> _paystackClientMock;
            private readonly Mock<PaystackApiKeys> _apiKeysMock;
            private readonly Mock<IRepository<User>> _userRepoMock;
            private readonly Mock<IRepository<Product>> _productRepoMock;
            private readonly Mock<IRepository<Transaction>> _paymentRepoMock;
            private readonly InitializeTransactionCommandHandler _handler;

            public InitializeTransactionCommandHandlerTests()
            {
                _paystackClientMock = new Mock<IPaystackClient>();
                _apiKeysMock = new Mock<PaystackApiKeys>();
                _userRepoMock = new Mock<IRepository<User>>();
                _productRepoMock = new Mock<IRepository<Product>>();
                _paymentRepoMock = new Mock<IRepository<Transaction>>();
                _handler = new InitializeTransactionCommandHandler(
                    _paystackClientMock.Object,
                    _apiKeysMock.Object,
                    _paymentRepoMock.Object,
                    _userRepoMock.Object,
                    _productRepoMock.Object);
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

                var user = new User
                {
                    Id = Guid.NewGuid(),
                    Email = "test@example.com",
                    FirstName = "John",
                    LastName = "Doe",
                    Password = "hashedpassword",
                    PasswordSalt = "salt"
                };
                var product = new Product()
                {
                    Id = Guid.NewGuid()
                };
                _userRepoMock.Setup(x => x.GetBySpec(It.IsAny<Expression<Func<User, bool>>>())).ReturnsAsync(user);
                _productRepoMock.Setup(x => x.GetBySpec(It.IsAny<Expression<Func<Product, bool>>>())).ReturnsAsync(product);
                _paystackClientMock
                    .Setup(x => x.InitializeTransaction(It.IsAny<InitializeTransactionRequest>()))
                    .ReturnsAsync(Result.Success(initializeResponse));

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
                var user = new User
                {
                    Id = Guid.NewGuid(),
                    Email = "test@example.com",
                    FirstName = "John",
                    LastName = "Doe",
                    Password = "hashedpassword",
                    PasswordSalt = "salt"
                };
                var product = new Product()
                {
                    Id = Guid.NewGuid()
                };
                _userRepoMock.Setup(x => x.GetBySpec(It.IsAny<Expression<Func<User, bool>>>())).ReturnsAsync(user);
                _productRepoMock.Setup(x => x.GetBySpec(It.IsAny<Expression<Func<Product, bool>>>())).ReturnsAsync(product);

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
}
