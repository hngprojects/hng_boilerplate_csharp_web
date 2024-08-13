using AutoMapper;
using Hng.Application.Features.BillingPlans.Handlers.Queries;
using Hng.Application.Features.BillingPlans.Mappers;
using Hng.Application.Features.BillingPlans.Queries;
using Hng.Domain.Entities;
using Hng.Domain.Enums;
using Hng.Infrastructure.Repository.Interface;
using Moq;
using Xunit;

namespace Hng.Application.Test.Features.BillingPlans
{
    public class GetBillingPlanByIdQueryShould
    {
        private readonly Mock<IRepository<BillingPlan>> _repositoryMock;
        private readonly IMapper _mapper;
        private readonly GetBillingPlanByIdQueryHandler _handler;

        public GetBillingPlanByIdQueryShould()
        {
            _repositoryMock = new Mock<IRepository<BillingPlan>>();

            var config = new MapperConfiguration(cfg => cfg.AddProfile<BillingPlanMapperProfile>());
            _mapper = config.CreateMapper();

            _handler = new GetBillingPlanByIdQueryHandler(_repositoryMock.Object, _mapper);
        }

        [Fact]
        public async Task Handle_ShouldReturnBillingPlanDto_WhenPlanExists()
        {
            // Arrange
            var billingPlanId = Guid.NewGuid();
            var billingPlan = new BillingPlan { Id = billingPlanId, Name = "Plan 1", Amount = 100, Frequency = SubscriptionFrequency.Monthly };

            _repositoryMock.Setup(repo => repo.GetAsync(billingPlanId))
                .ReturnsAsync(billingPlan);

            var query = new GetBillingPlanByIdQuery(billingPlanId);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(billingPlanId, result.Data.Id);
            Assert.Equal("Plan 1", result.Data.Name);
            Assert.Equal("Billing plan retrieved successfully.", result.Message);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailureResponse_WhenPlanDoesNotExist()
        {
            // Arrange
            var billingPlanId = Guid.NewGuid();

            _repositoryMock.Setup(repo => repo.GetAsync(billingPlanId))
                .ReturnsAsync((BillingPlan)null);

            var query = new GetBillingPlanByIdQuery(billingPlanId);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Null(result.Data);
            Assert.Equal("Billing plan not found.", result.Message);
        }
    }
}