using AutoMapper;
using Hng.Application.Features.Timezones.Commands;
using Hng.Application.Features.Timezones.Handlers.Commands;
using Hng.Application.Features.Timezones.Mappers;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using Moq;
using System.Linq.Expressions;
using Xunit;

namespace Hng.Application.Tests.Features.Timezones.Handlers.Commands
{
    public class CreateTimezoneCommandHandlerTests
    {
        private readonly IMapper _mapper;
        private readonly Mock<IRepository<Timezone>> _repositoryMock;
        private readonly CreateTimezoneCommandHandler _handler;

        public CreateTimezoneCommandHandlerTests()
        {
            var mappingProfile = new TimezoneMapperProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(mappingProfile));
            _mapper = new Mapper(configuration);

            _repositoryMock = new Mock<IRepository<Timezone>>();
            _handler = new CreateTimezoneCommandHandler(_repositoryMock.Object, _mapper);
        }

        [Fact]
        public async Task Handle_ShouldReturnCreatedTimezone()
        {
            // Arrange
            var expectedId = Guid.NewGuid();
            var createDto = new CreateTimezoneCommand
            {
                Timezone = "America/New_York",
                GmtOffset = "-06:00",
                Description = "Eastern Standard Time"
            };
            var timezone = new Timezone
            {
                Id = expectedId,
                TimezoneValue = createDto.Timezone,
                GmtOffset = createDto.GmtOffset,
                Description = createDto.Description
            };
            _repositoryMock.Setup(r => r.GetBySpec(It.IsAny<Expression<Func<Timezone, bool>>>()))
                .ReturnsAsync((Timezone)null);
            _repositoryMock.Setup(r => r.AddAsync(It.IsAny<Timezone>()))
                .ReturnsAsync(timezone);
            _repositoryMock.Setup(r => r.SaveChanges())
                .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(createDto, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(201, result.StatusCode);
            Assert.Equal("Timezone created successfully", result.Message);
            Assert.NotEqual(Guid.Empty, result.Timezone.Id);
            Assert.Equal(createDto.Timezone, result.Timezone.Timezone);
            Assert.Equal(createDto.GmtOffset, result.Timezone.GmtOffset);
            Assert.Equal(createDto.Description, result.Timezone.Description);
        }

        [Fact]
        public async Task Handle_ShouldReturnConflictWhenTimezoneAlreadyExists()
        {
            // Arrange
            var existingTimezone = new Timezone
            {
                Id = Guid.NewGuid(),
                TimezoneValue = "America/New_York",
                GmtOffset = "-05:00",
                Description = "Eastern Standard Time"
            };

            var createDto = new CreateTimezoneCommand
            {
                Timezone = "America/New_York",
                GmtOffset = "-05:00",
                Description = "Eastern Standard Time"
            };

            _repositoryMock.Setup(r => r.GetBySpec(It.IsAny<System.Linq.Expressions.Expression<Func<Timezone, bool>>>()))
                .ReturnsAsync(existingTimezone);

            // Act
            var result = await _handler.Handle(createDto, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(409, result.StatusCode);
            Assert.Equal($"Timezone '{createDto.Timezone}' already exists.", result.Error);
            Assert.Equal("Timezone already exists", result.Message);
        }
    }
}