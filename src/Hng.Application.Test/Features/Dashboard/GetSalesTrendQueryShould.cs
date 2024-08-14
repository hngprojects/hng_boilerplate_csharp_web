using AutoMapper;
using FluentAssertions;
using Hng.Application.Features.Dashboard.Handlers;
using Hng.Application.Features.Dashboard.Queries;
using Hng.Application.Features.ExternalIntegrations.PaymentIntegrations.Paystack.Dtos.Responses;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using Hng.Infrastructure.Services.Interfaces;
using Moq;
using System.Linq.Expressions;
using Xunit;

namespace Hng.Application.Test.Features.Dashboard
{
    public class GetNavigationDataQueryShould
    {
        private readonly Mock<IRepository<Transaction>> _transactionRepositoryMock;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IAuthenticationService> _authenticationServiceMock;
        private readonly GetNavigationDataQueryHandler _handler;

        public GetNavigationDataQueryShould()
        {
            _transactionRepositoryMock = new Mock<IRepository<Transaction>>();

            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Transaction, TransactionDto>();
            });
            _authenticationServiceMock = new Mock<IAuthenticationService>();
            _mockMapper = new Mock<IMapper>();
            _handler = new GetNavigationDataQueryHandler(_transactionRepositoryMock.Object, _mockMapper.Object, _authenticationServiceMock.Object);
        }

        [Fact]
        public async Task Handle_NavigationRecordExist()
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
                    CreatedAt = DateTime.Now.AddMonths(-2)
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
                    CreatedAt =DateTime.Now
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
                    PaidAt = DateTime.Now.AddDays(-2),
                    CreatedAt = DateTime.Now.AddMonths(-2)
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
                    PaidAt = DateTime.Now.AddDays(-2),
                    CreatedAt = DateTime.Now
                }
            };

            _authenticationServiceMock.Setup(s => s.GetCurrentUserAsync()).ReturnsAsync(userId);
            _transactionRepositoryMock.Setup(r => r.GetAllBySpec(It.IsAny<Expression<Func<Transaction, bool>>>()))
               .ReturnsAsync(transaction);

            _mockMapper.Setup(m => m.Map<IEnumerable<TransactionDto>>(It.IsAny<IEnumerable<Transaction>>()))
                .Returns(transactionDto);
            var query = new GetNavigationDataQuery();

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task Handle_NoRecordExist_ReturnsEmptyList()
        {
            // Arrange
            var userId = Guid.NewGuid();
            _authenticationServiceMock.Setup(s => s.GetCurrentUserAsync()).ReturnsAsync(userId);
            _transactionRepositoryMock.Setup(repo => repo.GetAllBySpec(x => x.UserId == userId))
               .ReturnsAsync(new List<Transaction>());
            var query = new GetNavigationDataQuery();

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().BeNull();
        }
    }
}
