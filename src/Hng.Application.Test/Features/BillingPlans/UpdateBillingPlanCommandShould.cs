using AutoMapper;
using Hng.Application.Features.BillingPlans.Commands;
using Hng.Application.Features.BillingPlans.Dtos;
using Hng.Application.Features.BillingPlans.Handlers.Commands;
using Hng.Application.Features.BillingPlans.Mappers;
using Hng.Domain.Entities;
using Hng.Domain.Enums;
using Hng.Infrastructure.Repository.Interface;
using Moq;
using Xunit;

namespace Hng.Application.Test.Features.BillingPlans
{
    public class UpdateBillingPlanCommandShould
    {
        private readonly Mock<IRepository<BillingPlan>> _repositoryMock;
        private readonly IMapper _mapper;
        private readonly UpdateBillingPlanCommandHandler _handler;

        public UpdateBillingPlanCommandShould()
        {
            _repositoryMock = new Mock<IRepository<BillingPlan>>();

            var config = new MapperConfiguration(cfg => cfg.AddProfile<BillingPlanMapperProfile>());
            _mapper = config.CreateMapper();

            _handler = new UpdateBillingPlanCommandHandler(_repositoryMock.Object, _mapper);
        }

        [Fact]
        public async Task Handle_ShouldReturnUpdatedBillingPlanDto_WhenUpdateIsSuccessful()
        {
            // Arrange
            var billingPlanId = Guid.NewGuid();
            var updateDto = new CreateBillingPlanDto { Name = "Updated Plan", Amount = 150, Frequency = SubscriptionFrequency.Monthly };

            var existingPlan = new BillingPlan
            {
                Id = billingPlanId,
                Name = "Old Plan",
                Amount = 100,
                Frequency = SubscriptionFrequency.Monthly
            };

            _repositoryMock.Setup(repo => repo.GetAsync(billingPlanId))
                .ReturnsAsync(existingPlan);

            _repositoryMock.Setup(repo => repo.UpdateAsync(It.IsAny<BillingPlan>()))
                .Returns(Task.CompletedTask);

            var command = new UpdateBillingPlanCommand(billingPlanId, updateDto);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(updateDto.Name, result.Data.Name);
            Assert.Equal(updateDto.Amount, result.Data.Amount);
            Assert.Equal(updateDto.Frequency, result.Data.Frequency);
            Assert.Equal("Billing plan updated successfully.", result.Message);

            _repositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<BillingPlan>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailureResponse_WhenPlanDoesNotExist()
        {
            // Arrange
            var billingPlanId = Guid.NewGuid();
            var updateDto = new CreateBillingPlanDto { Name = "Updated Plan", Amount = 150, Frequency = SubscriptionFrequency.Monthly };

            _repositoryMock.Setup(repo => repo.GetAsync(billingPlanId))
                .ReturnsAsync((BillingPlan)null);

            var command = new UpdateBillingPlanCommand(billingPlanId, updateDto);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Null(result.Data);
            Assert.Equal("Billing plan not found.", result.Message);
        }
    }
}