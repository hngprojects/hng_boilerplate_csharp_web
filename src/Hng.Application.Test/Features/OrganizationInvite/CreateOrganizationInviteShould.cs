using AutoMapper;
using Hng.Application.Features.OrganisationInvite.Commands;
using Hng.Application.Features.OrganisationInvite.Dtos;
using Hng.Application.Features.OrganisationInvite.Handlers;
using Hng.Application.Features.OrganisationInvite.Mappers;
using Hng.Domain.Enums;
using Hng.Infrastructure.Services.Interfaces;
using Moq;
using Xunit;

namespace Hng.Application.Test.Features.OrganizationInvite
{
    public class CreateOrganizationInviteShould
    {
        private readonly IMapper _mapper;
        private readonly Mock<IOrganizationInviteService> _serviceMock;
        private readonly CreateOrganizationInviteCommandHandler _handler;

        public CreateOrganizationInviteShould()
        {
            var mappingProfile = new OrganizationInviteMapperProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(mappingProfile));
            _mapper = new Mapper(configuration);

            _serviceMock = new Mock<IOrganizationInviteService>();
            _handler = new CreateOrganizationInviteCommandHandler(_serviceMock.Object, _mapper);
        }

        [Fact]
        public async Task Handle_ShouldReturnCreatedOrganizationInvite()
        {
            // Arrange
            var expectedId = Guid.NewGuid();
            var organizationId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var createDto = new CreateOrganizationInviteDto
            {
                OrganizationId = organizationId.ToString(),
                UserId = userId,
                Email = "test@example.com"
            };

            var organizationInvite = new Domain.Entities.OrganizationInvite
            {
                Id = expectedId,
                OrganizationId = organizationId,
                Email = createDto.Email,
                Status = OrganizationInviteStatus.Pending,
                InviteLink = "inviteLink",
                CreatedAt = DateTimeOffset.UtcNow,
                ExpiresAt = DateTimeOffset.UtcNow.AddDays(7)
            };

            _serviceMock.Setup(s => s.CreateInvite(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<string>()))
                .ReturnsAsync(organizationInvite);

            var command = new CreateOrganizationInviteCommand(createDto);

            // Act
            var result = await _handler.Handle(command, default);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedId.ToString(), result.Id);
            Assert.Equal(organizationId.ToString(), result.OrganizationId);
            Assert.Equal(createDto.Email, result.Email);
            Assert.Equal(OrganizationInviteStatus.Pending.ToString(), result.Status);
            Assert.Equal(organizationInvite.InviteLink, result.InviteLink);
            Assert.Equal(organizationInvite.ExpiresAt, result.ExpiresAt);

            _serviceMock.Verify(s => s.CreateInvite(userId, organizationId, createDto.Email), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnNull_WhenInvalidOrganizationId()
        {
            // Arrange
            var createDto = new CreateOrganizationInviteDto
            {
                OrganizationId = "invalid-guid",
                UserId = Guid.NewGuid(),
                Email = "test@example.com"
            };

            var command = new CreateOrganizationInviteCommand(createDto);

            // Act
            var result = await _handler.Handle(command, default);

            // Assert
            Assert.Null(result);
            _serviceMock.Verify(s => s.CreateInvite(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<string>()), Times.Never);
        }
    }
}