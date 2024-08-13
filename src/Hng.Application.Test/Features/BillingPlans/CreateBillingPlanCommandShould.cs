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
    public class CreateBillingPlanCommandShould
    {
        private readonly Mock<IRepository<BillingPlan>> _repositoryMock;
        private readonly IMapper _mapper;
        private readonly CreateBillingPlanCommandHandler _handler;

        public CreateBillingPlanCommandShould()
        {
            _repositoryMock = new Mock<IRepository<BillingPlan>>();

            var config = new MapperConfiguration(cfg => cfg.AddProfile<BillingPlanMapperProfile>());
            _mapper = config.CreateMapper();

            _handler = new CreateBillingPlanCommandHandler(_repositoryMock.Object, _mapper);
        }

        [Fact]
        public async Task Handle_ShouldReturnBillingPlanDto_WhenRequestIsValid()
        {
            // Arrange
            var billingPlanRequest = new CreateBillingPlanDto
            {
                Name = "Test Plan",
                Amount = 100,
                Frequency = SubscriptionFrequency.Monthly,
                Description = "Test Description"
            };
            var command = new CreateBillingPlanCommand(billingPlanRequest);

            var createdBillingPlan = new BillingPlan
            {
                Id = Guid.NewGuid(),
                Name = billingPlanRequest.Name,
                Frequency = billingPlanRequest.Frequency,
                Amount = billingPlanRequest.Amount,
                Description = billingPlanRequest.Description,
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };

            _repositoryMock.Setup(repo => repo.AddAsync(It.IsAny<BillingPlan>()))
                .ReturnsAsync(createdBillingPlan);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Data);
            Assert.Equal(billingPlanRequest.Name, result.Data.Name);
            Assert.Equal(billingPlanRequest.Frequency, result.Data.Frequency);
            Assert.Equal(billingPlanRequest.Amount, result.Data.Amount);
            Assert.Equal("Billing plan created successfully.", result.Message);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailureResponse_WhenCreationFails()
        {
            // Arrange
            var billingPlanRequest = new CreateBillingPlanDto
            {
                Name = "Test Plan",
                Amount = 100,
                Frequency = SubscriptionFrequency.Monthly,
                Description = "Test Description"
            };
            var command = new CreateBillingPlanCommand(billingPlanRequest);

            _repositoryMock.Setup(repo => repo.AddAsync(It.IsAny<BillingPlan>()))
                .ReturnsAsync((BillingPlan)null); // Simulate a failure in creation

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result); // Ensure the result is not null
            Assert.Null(result.Data); // Check that Data is null
            Assert.Equal("Failed to create billing plan.", result.Message);
        }
    }
}