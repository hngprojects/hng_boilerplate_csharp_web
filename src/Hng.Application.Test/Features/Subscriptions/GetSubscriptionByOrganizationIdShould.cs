using AutoMapper;
using Hng.Application.Features.Subscriptions.Dtos.Requests;
using Hng.Application.Features.Subscriptions.Dtos.Responses;
using Hng.Application.Features.Subscriptions.Handlers.Queries;
using Hng.Domain.Entities;
using Hng.Domain.Enums;
using Hng.Infrastructure.Repository.Interface;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Hng.Application.Test.Features.Subscriptions
{
    public class GetSubscriptionByOrganizationIdShould
    {
        private readonly Mock<IRepository<Subscription>> _mockRepository;
        private readonly IMapper _mapper;

        public GetSubscriptionByOrganizationIdShould()
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
            var organizationId = Guid.NewGuid();
            var subscription = new Subscription { OrganizationId = organizationId, Plan = SubscriptionPlan.Premium };
            _mockRepository.Setup(repo => repo.GetBySpec(It.IsAny<Expression<Func<Subscription, bool>>>()))
                .ReturnsAsync(subscription);
            var handler = new GetSubscriptionByOrganisationIdQueryHandler(_mockRepository.Object, _mapper);
            var query = new GetSubscriptionByOrganizationIdQuery(organizationId);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(organizationId, result.OrganizationId);
            Assert.Equal("Premium", result.Plan);
        }

        [Fact]
        public async Task ReturnNull_WhenSubscriptionNotFound()
        {
            // Arrange
            var organizationId = Guid.NewGuid();
            _mockRepository.Setup(repo => repo.GetBySpec(It.IsAny<Expression<Func<Subscription, bool>>>()))
                .ReturnsAsync((Subscription)null);

            var handler = new GetSubscriptionByOrganisationIdQueryHandler(_mockRepository.Object, _mapper);
            var query = new GetSubscriptionByOrganizationIdQuery(organizationId);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Null(result);
        }
    }
}