using AutoMapper;
using Hng.Application.Features.Timezones.Commands;
using Hng.Application.Features.Timezones.Handlers.Commands;
using Hng.Application.Features.Timezones.Mappers;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using Moq;
using Xunit;

namespace Hng.Application.Test.Features.Timezones
{
    public class UpdateTimezoneShould
    {
        private readonly IMapper _mapper;
        private readonly Mock<IRepository<Timezone>> _repositoryMock;
        private readonly UpdateTimezoneCommandHandler _handler;

        public UpdateTimezoneShould()
        {
            var mappingProfile = new TimezoneMapperProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(mappingProfile));
            _mapper = new Mapper(configuration);
            _repositoryMock = new Mock<IRepository<Timezone>>();
            _handler = new UpdateTimezoneCommandHandler(_repositoryMock.Object, _mapper);
        }

        [Fact]
        public async Task Handle_ShouldReturnUpdatedTimezone()
        {
            // Arrange
            var existingId = Guid.NewGuid();
            var updateCommand = new UpdateTimezoneCommand
            {
                Id = existingId,
                Timezone = "America/New_York",
                GmtOffset = "-05:00",
                Description = "Eastern Standard Time (Updated)"
            };

            var existingTimezone = new Timezone
            {
                Id = existingId,
                TimezoneValue = "America/Chicago",
                GmtOffset = "-06:00",
                Description = "Central Standard Time"
            };

            _repositoryMock.Setup(r => r.GetAsync(existingId))
                .ReturnsAsync(existingTimezone);

            _repositoryMock.Setup(r => r.UpdateAsync(It.IsAny<Timezone>()))
                .Returns(Task.CompletedTask);

            _repositoryMock.Setup(r => r.SaveChanges())
                .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(updateCommand, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal("Timezone updated successfully", result.Message);
            Assert.Equal(updateCommand.Timezone, result.Timezone.Timezone);
            Assert.Equal(updateCommand.GmtOffset, result.Timezone.GmtOffset);
            Assert.Equal(updateCommand.Description, result.Timezone.Description);
        }

        [Fact]
        public async Task Handle_ShouldReturnNotFoundWhenTimezoneDoesNotExist()
        {
            // Arrange
            var nonExistentId = Guid.NewGuid();
            var updateCommand = new UpdateTimezoneCommand
            {
                Id = nonExistentId,
                Timezone = "America/New_York",
                GmtOffset = "-05:00",
                Description = "Eastern Standard Time"
            };

            _repositoryMock.Setup(r => r.GetAsync(nonExistentId))
                .ReturnsAsync((Timezone)null);

            // Act
            var result = await _handler.Handle(updateCommand, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(404, result.StatusCode);
            Assert.Equal("Timezone not found", result.Message);
        }
    }
}