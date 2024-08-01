using Hng.Application.Features.Subscriptions.Dtos.Requests;
using Hng.Application.Features.Subscriptions.Handlers.Commands;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using Moq;
using Xunit;

namespace Hng.Application.Test.Features.Subscriptions
{
    public class CreateFreeSubscriptionPlanShould
    {
        private readonly Mock<IRepository<Subscription>> _subRepoMock;
        private readonly CreateFreeSubscriptionPlanHandler _handler;

        public CreateFreeSubscriptionPlanShould()
        {
            _subRepoMock = new Mock<IRepository<Subscription>>();
            _handler = new CreateFreeSubscriptionPlanHandler(_subRepoMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnCreatedSubscription()
        {
            var expectedId = Guid.NewGuid();
            var createDto = new SubscribeFreePlan
            {
                UserId = Guid.NewGuid().ToString()
            };

            var subscription = new Subscription
            {
                Id = expectedId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };

            _subRepoMock.Setup(r => r.AddAsync(It.IsAny<Subscription>()))
                .ReturnsAsync(subscription);

            var result = await _handler.Handle(createDto, default);

            Assert.NotNull(result.Value);
            Assert.True(result.IsSuccess);
        }
    }
}
