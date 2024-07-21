using AutoMapper;
using Hng.Application.Dto;
using Hng.Application.Services;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using Moq;
using Xunit;

namespace Hng.Application.Test;

public class TestSetupShould
{
    private readonly Mock<ISubscriptionPlanRepository> _mockRepo;
    private readonly Mock<IMapper> _mockMapper;
    private readonly SubscriptionPlanService _service;

    public TestSetupShould()
    {
        _mockRepo = new Mock<ISubscriptionPlanRepository>();
        _mockMapper = new Mock<IMapper>();
       // _service = new SubscriptionPlanService(_mockRepo.Object, _mockMapper.Object);
    }

    [Fact]
    public void Pass_For_Setup_Sake()
    {
        Assert.False(1 * 1 == 2, "1 times 1 should not be equal to 2");
    }

        [Fact]
        public async Task CreatePlanAsync_WithValidDto_ReturnsCreatedPlan()
        {
            // Arrange
            var dto = new CreateSubscriptionPlanDto { Name = "Test Plan", Price = 100 };
           // var plan = new SubscriptionPlan { Id = 1, Name = "Test Plan", Price = 100 };
            _mockRepo.Setup(repo => repo.ExistsAsync(It.IsAny<string>())).ReturnsAsync(false);
         //   _mockRepo.Setup(repo => repo.CreateAsync(It.IsAny<SubscriptionPlan>())).ReturnsAsync(plan);
         //   _mockMapper.Setup(m => m.Map<SubscriptionPlan>(It.IsAny<CreateSubscriptionPlanDto>())).Returns(plan);

            // Act
            var result = await _service.CreatePlanAsync(dto);

            // Assert
            Assert.NotNull(result);
        //    Assert.Equal(plan.Id, result.Id);
         //   Assert.Equal(plan.Name, result.Name);
        }

        [Fact]
        public async Task CreatePlanAsync_WithExistingPlan_ThrowsException()
        {
            // Arrange
            var dto = new CreateSubscriptionPlanDto { Name = "Existing Plan" };
            _mockRepo.Setup(repo => repo.ExistsAsync(It.IsAny<string>())).ReturnsAsync(true);

            // Act & Assert
            await Assert.ThrowsAsync<ApplicationException>(() => _service.CreatePlanAsync(dto));
        }
    }

