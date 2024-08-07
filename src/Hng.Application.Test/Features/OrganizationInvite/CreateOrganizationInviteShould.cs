using Xunit;
using Moq;
using AutoMapper;
using Hng.Application.Features.OrganisationInvite.Commands;
using Hng.Application.Features.OrganisationInvite.Handlers;
using Hng.Application.Features.OrganisationInvite.Dtos;
using Hng.Domain.Entities;
using Hng.Infrastructure.Services.Interfaces;
using Hng.Domain.Common;
using Hng.Infrastructure.Utilities.Errors.OrganisationInvite;

namespace Hng.Application.Tests.Features.OrganisationInvite.Handlers
{
    public class CreateOrganizationInviteCommandHandlerTests
    {
        private readonly Mock<IOrganizationInviteService> _mockService;
        private readonly Mock<IMapper> _mockMapper;
        private readonly CreateOrganizationInviteCommandHandler _handler;

        public CreateOrganizationInviteCommandHandlerTests()
        {
            _mockService = new Mock<IOrganizationInviteService>();
            _mockMapper = new Mock<IMapper>();
            _handler = new CreateOrganizationInviteCommandHandler(_mockService.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task Handle_WithValidRequest_ShouldReturnSuccessResult()
        {
            // Arrange
            var command = new CreateOrganizationInviteCommand(new CreateOrganizationInviteDto
            {
                OrganizationId = Guid.NewGuid().ToString(),
                UserId = Guid.NewGuid(),
                Email = "test@example.com"
            });

            var organizationInvite = new OrganizationInvite();
            var expectedDto = new OrganizationInviteDto();

            _mockService.Setup(s => s.CreateInvite(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<string>()))
                .ReturnsAsync(Result<OrganizationInvite>.Success(organizationInvite));

            _mockMapper.Setup(m => m.Map<OrganizationInviteDto>(organizationInvite))
                .Returns(expectedDto);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(expectedDto, result.Value);
        }

        [Fact]
        public async Task Handle_WithInvalidOrganizationId_ShouldReturnFailureResult()
        {
            // Arrange
            var command = new CreateOrganizationInviteCommand(new CreateOrganizationInviteDto
            {
                OrganizationId = "invalid-guid",
                UserId = Guid.NewGuid(),
                Email = "test@example.com"
            });

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.True(result.Error is not null);
        }

        [Fact]
        public async Task Handle_WithServiceFailure_ShouldReturnFailureResult()
        {
            // Arrange
            var command = new CreateOrganizationInviteCommand(new CreateOrganizationInviteDto
            {
                OrganizationId = Guid.NewGuid().ToString(),
                UserId = Guid.NewGuid(),
                Email = "test@example.com"
            });

            var expectedError = new Error("Service error");

            _mockService.Setup(s => s.CreateInvite(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<string>()))
                .ReturnsAsync(Result<OrganizationInvite>.Failure(expectedError));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.Equal("Service error", result.Error.Message);
            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(expectedError.Message, result.Error.Message);
        }
    }
}