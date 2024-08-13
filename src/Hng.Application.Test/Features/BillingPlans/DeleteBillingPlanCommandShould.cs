using Hng.Application.Features.BillingPlans.Commands;
using Hng.Application.Features.BillingPlans.Handlers.Commands;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using Moq;
using Xunit;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Hng.Application.Test.Features.BillingPlans
{
    public class DeleteBillingPlanCommandShould
    {
        private readonly Mock<IRepository<BillingPlan>> _repositoryMock;
        private readonly DeleteBillingPlanCommandHandler _handler;

        public DeleteBillingPlanCommandShould()
        {
            _repositoryMock = new Mock<IRepository<BillingPlan>>();
            _handler = new DeleteBillingPlanCommandHandler(_repositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnTrue_WhenDeletionIsSuccessful()
        {
            // Arrange
            var billingPlanId = Guid.NewGuid();
            var existingPlan = new BillingPlan { Id = billingPlanId, Name = "Plan to Delete" };

            _repositoryMock.Setup(repo => repo.GetAsync(billingPlanId))
                .ReturnsAsync(existingPlan);

            _repositoryMock.Setup(repo => repo.DeleteAsync(It.IsAny<BillingPlan>()))
                .Returns(Task.FromResult(existingPlan));

            var command = new DeleteBillingPlanCommand(billingPlanId);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            //Assert.True(result);
            // _repositoryMock.Verify(x => x.DeleteAsync(It.Is<BillingPlan>(bp => bp.Id == billingPlanId)), Times.Once);

            //var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.Data);
            Assert.NotNull(result);
            Assert.Equal("Billing plan deleted successfully.", result.Message);


        }

        [Fact]
        public async Task Handle_ShouldReturnFalse_WhenPlanDoesNotExist()
        {
            // Arrange
            var billingPlanId = Guid.NewGuid();

            _repositoryMock.Setup(repo => repo.GetAsync(billingPlanId))
                .ReturnsAsync((BillingPlan)null);

            var command = new DeleteBillingPlanCommand(billingPlanId);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.Data);
        }
    }
}