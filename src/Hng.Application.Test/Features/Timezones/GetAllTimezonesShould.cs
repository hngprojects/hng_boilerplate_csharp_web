using AutoMapper;
using Hng.Application.Features.Timezones.Dtos;
using Hng.Application.Features.Timezones.Handlers.Queries;
using Hng.Application.Features.Timezones.Queries;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using Moq;
using Xunit;

namespace Hng.Application.Test.Features.Timezones
{
    public class GetAllTimezonesShould
    {
        private readonly Mock<IRepository<Timezone>> _timezoneRepositoryMock;
        private readonly IMapper _mapper;
        private readonly GetAllTimezonesQueryHandler _handler;

        public GetAllTimezonesShould()
        {
            _timezoneRepositoryMock = new Mock<IRepository<Timezone>>();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Timezone, TimezoneDto>();
            });
            _mapper = config.CreateMapper();

            _handler = new GetAllTimezonesQueryHandler(_timezoneRepositoryMock.Object, _mapper);
        }

        [Fact]
        public async Task Handle_ReturnsPaginatedResponse_WhenTimezonesExist()
        {
            // Arrange
            var timezones = new List<Timezone>
        {
            new Timezone { Id = Guid.NewGuid(), TimezoneValue = "UTC", GmtOffset = "+00:00", Description = "Coordinated Universal Time" },
            new Timezone { Id = Guid.NewGuid(), TimezoneValue = "PST", GmtOffset = "-08:00", Description = "Pacific Standard Time" }
        };
            _timezoneRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(timezones);
            var query = new GetAllTimezonesQuery { PageNumber = 1, PageSize = 10 };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Data.Count);
            Assert.Equal(1, result.Metadata.CurrentPage);
            Assert.Equal(2, result.Metadata.TotalCount);
        }

        [Fact]
        public async Task Handle_ReturnsEmptyPaginatedResponse_WhenNoTimezonesExist()
        {
            // Arrange
            _timezoneRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(new List<Timezone>());
            var query = new GetAllTimezonesQuery { PageNumber = 1, PageSize = 10 };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result.Data);
            Assert.Equal(0, result.Metadata.TotalCount);
        }
    }
}