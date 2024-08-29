using AutoMapper;
using Hng.Application.Features.Subscriptions.Dtos.Requests;
using Hng.Application.Features.Subscriptions.Dtos.Responses;
using Hng.Application.Features.Subscriptions.Handlers.Queries;
using Hng.Domain.Entities;
using Hng.Domain.Enums;
using Hng.Infrastructure.Repository.Interface;
using Moq;
using System.Linq.Expressions;
using Xunit;

namespace Hng.Application.Test.Features.Subscriptions
{
    public class GetSubscriptionByUserIdShould
    {
        private readonly Mock<IRepository<Subscription>> _mockRepository;
        private readonly IMapper _mapper;

        public GetSubscriptionByUserIdShould()
        {
            _mockRepository = new Mock<IRepository<Subscription>>();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Subscription, SubscriptionDto>();
            });
            _mapper = config.CreateMapper();
        }

        [Fact]
        public async Task ReturnSubscriptionDto_WhenSubscriptionExists()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var subscription = new Subscription { UserId = userId, Plan = SubscriptionPlan.Free };
            _mockRepository.Setup(repo => repo.GetBySpec(It.IsAny<Expression<Func<Subscription, bool>>>()))
                .ReturnsAsync(subscription);
            var handler = new GetSubscriptionByUserIdQueryHandler(_mockRepository.Object, _mapper);
            var query = new GetSubscriptionByUserIdQuery(userId);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(userId, result.UserId);
            Assert.Equal("Free", result.Plan);
        }

        [Fact]
        public async Task ReturnNull_WhenSubscriptionNotFound()
        {
            // Arrange
            var userId = Guid.NewGuid();
            _mockRepository.Setup(repo => repo.GetBySpec(It.IsAny<Expression<Func<Subscription, bool>>>()))
                .ReturnsAsync((Subscription)null);
            var handler = new GetSubscriptionByUserIdQueryHandler(_mockRepository.Object, _mapper);
            var query = new GetSubscriptionByUserIdQuery(userId);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Null(result);
        }
    }
}
