using AutoMapper;
using Hng.Application.Features.BillingPlans.Handlers.Queries;
using Hng.Application.Features.BillingPlans.Mappers;
using Hng.Application.Features.BillingPlans.Queries;
using Hng.Application.Shared.Dtos;
using Hng.Domain.Entities;
using Hng.Domain.Enums;
using Hng.Infrastructure.Repository.Interface;
using Moq;
using Xunit;

namespace Hng.Application.Test.Features.BillingPlans
{
    public class GetAllBillingPlansQueryShould
    {
        private readonly Mock<IRepository<BillingPlan>> _repositoryMock;
        private readonly IMapper _mapper;
        private readonly GetAllBillingPlansQueryHandler _handler;

        public GetAllBillingPlansQueryShould()
        {
            _repositoryMock = new Mock<IRepository<BillingPlan>>();

            var config = new MapperConfiguration(cfg => cfg.AddProfile<BillingPlanMapperProfile>());
            _mapper = config.CreateMapper();

            _handler = new GetAllBillingPlansQueryHandler(_repositoryMock.Object, _mapper);
        }

        [Fact]
        public async Task Handle_ShouldReturnListOfBillingPlanDtos_WhenPlansExist()
        {
            // Arrange
            var billingPlans = new List<BillingPlan>
            {
                new BillingPlan { Id = Guid.NewGuid(), Name = "Plan 1", Amount = 100, Frequency = SubscriptionFrequency.Monthly, CreatedAt = DateTime.UtcNow },
                new BillingPlan { Id = Guid.NewGuid(), Name = "Plan 2", Amount = 200, Frequency = SubscriptionFrequency.Monthly, CreatedAt = DateTime.UtcNow }
            };

            _repositoryMock.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(billingPlans);

            var queryParameters = new BaseQueryParameters { Limit = 10, Offset = 1 };
            var query = new GetAllBillingPlansQuery(queryParameters);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Data.Count); // Assuming the result is a PaginatedResponseDto
            Assert.Equal("Plan 1", result.Data[0].Name);
            Assert.Equal("Plan 2", result.Data[1].Name);
        }

        [Fact]
        public async Task Handle_ShouldReturnEmptyList_WhenNoPlansExist()
        {
            // Arrange
            _repositoryMock.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(new List<BillingPlan>());

            var queryParameters = new BaseQueryParameters { Limit = 10, Offset = 1 };
            var query = new GetAllBillingPlansQuery(queryParameters);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result.Data); // Assuming the result is a PaginatedResponseDto
        }
    }
}