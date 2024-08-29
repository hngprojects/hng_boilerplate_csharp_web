using AutoMapper;
using FluentAssertions;
using Hng.Application.Features.ExternalIntegrations.PaymentIntegrations.Paystack.Dtos.Responses;
using Hng.Application.Features.Dashboard.Handlers;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using Hng.Infrastructure.Services.Interfaces;
using Moq;
using Xunit;
using Hng.Application.Features.Subscriptions.Dtos.Responses;
using Hng.Application.Features.Dashboard.Queries;
using System.Linq.Expressions;

namespace Hng.Application.Test.Features.Dashboard
{
    public class GetDashboardQueryShould
    {
        private readonly Mock<IRepository<Transaction>> _transactionRepositoryMock;
        private readonly Mock<IRepository<Subscription>> _subscriptionRepositoryMock;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IAuthenticationService> _authenticationServiceMock;
        private readonly GetDashboardQueryHandler _handler;

        public GetDashboardQueryShould()
        {
            _transactionRepositoryMock = new Mock<IRepository<Transaction>>();
            _subscriptionRepositoryMock = new Mock<IRepository<Subscription>>();

            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Transaction, TransactionDto>();
                cfg.CreateMap<Subscription, SubscriptionDto>();
            });
            _authenticationServiceMock = new Mock<IAuthenticationService>();
            _mockMapper = new Mock<IMapper>();
            _handler = new GetDashboardQueryHandler(_transactionRepositoryMock.Object, _subscriptionRepositoryMock.Object, _mockMapper.Object, _authenticationServiceMock.Object);
        }

        [Fact]
        public async Task Handle_DashboardsExist()
        {
            // Arrange
            var userId = Guid.NewGuid();

            var transaction = new List<Transaction>
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    ProductId = Guid.NewGuid(),
                    UserId = userId,
                    SubscriptionId = Guid.NewGuid(),
                    Type = Domain.Enums.TransactionType.subscription,
                    Status = Domain.Enums.TransactionStatus.Completed,
                    Partners = Domain.Enums.TransactionIntegrationPartners.Flutterwave,
                    Amount = 100,
                    PaidAt = DateTime.Now,
                    CreatedAt = DateTime.Now
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    ProductId = Guid.NewGuid(),
                    UserId = userId,
                    SubscriptionId = Guid.NewGuid(),
                    Type = Domain.Enums.TransactionType.subscription,
                    Status = Domain.Enums.TransactionStatus.Completed,
                    Partners = Domain.Enums.TransactionIntegrationPartners.Flutterwave,
                    Amount = 100,
                    PaidAt = DateTime.Now,
                    CreatedAt = DateTime.Now
                }
            };

            var transactionDto = new List<TransactionDto>
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    ProductId = Guid.NewGuid(),
                    UserId = userId,
                    SubscriptionId = Guid.NewGuid(),
                    Type = Domain.Enums.TransactionType.subscription,
                    Status = Domain.Enums.TransactionStatus.Completed,
                    Partners = Domain.Enums.TransactionIntegrationPartners.Flutterwave,
                    Amount = 100,
                    PaidAt = DateTime.Now,
                    CreatedAt = DateTime.Now
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    ProductId = Guid.NewGuid(),
                    UserId = userId,
                    SubscriptionId = Guid.NewGuid(),
                    Type = Domain.Enums.TransactionType.subscription,
                    Status = Domain.Enums.TransactionStatus.Completed,
                    Partners = Domain.Enums.TransactionIntegrationPartners.Flutterwave,
                    Amount = 100,
                    PaidAt = DateTime.Now,
                    CreatedAt = DateTime.Now
                }
            };

            var subscription = new List<Subscription>
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    OrganizationId = Guid.NewGuid(),
                    TransactionId = Guid.NewGuid(),
                    BillingPlanId = Guid.NewGuid(),
                    Plan = Domain.Enums.SubscriptionPlan.Premium,
                    Frequency = Domain.Enums.SubscriptionFrequency.Annually,
                    IsActive = true,
                    Amount = 100,
                    CreatedAt = DateTime.Now,
                    ExpiryDate = DateTime.Now.AddYears(1)
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    OrganizationId = Guid.NewGuid(),
                    TransactionId = Guid.NewGuid(),
                    BillingPlanId = Guid.NewGuid(),
                    Plan = Domain.Enums.SubscriptionPlan.Premium,
                    Frequency = Domain.Enums.SubscriptionFrequency.Annually,
                    IsActive = true,
                    Amount = 100,
                    CreatedAt = DateTime.Now,
                    ExpiryDate = DateTime.Now.AddYears(1)
                }
            };

            _authenticationServiceMock.Setup(s => s.GetCurrentUserAsync()).ReturnsAsync(userId);
            _transactionRepositoryMock.Setup(r => r.GetAllBySpec(It.IsAny<Expression<Func<Transaction, bool>>>()))
               .ReturnsAsync(transaction);
            _subscriptionRepositoryMock.Setup(r => r.GetAllBySpec(It.IsAny<Expression<Func<Subscription, bool>>>()))
               .ReturnsAsync(subscription);

            _mockMapper.Setup(m => m.Map<IEnumerable<TransactionDto>>(It.IsAny<IEnumerable<Transaction>>()))
                .Returns(transactionDto);
            var query = new GetDashboardQuery(userId);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            Assert.Equal(200, result.Revenue);
            Assert.Equal(2, result.Sales);
            Assert.Equal(2, result.ActiveSubscription);
        }

        [Fact]
        public async Task Handle_NoRecordExist_ReturnsEmptyList()
        {
            // Arrange
            var userId = Guid.NewGuid();
            _authenticationServiceMock.Setup(s => s.GetCurrentUserAsync()).ReturnsAsync(userId);
            _transactionRepositoryMock.Setup(repo => repo.GetAllBySpec(x => x.UserId == userId))
               .ReturnsAsync(new List<Transaction>());
            _subscriptionRepositoryMock.Setup(repo => repo.GetAllBySpec(x => x.UserId == userId))
                .ReturnsAsync(new List<Subscription>());
            var query = new GetDashboardQuery(userId);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().BeNull();
        }
    }
}
