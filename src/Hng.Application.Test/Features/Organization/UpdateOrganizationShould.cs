using AutoMapper;
using Hng.Application.Features.Organisations.Commands;
using Hng.Application.Features.Organisations.Dtos;
using Hng.Application.Features.Organisations.Handlers;
using Hng.Application.Features.Organisations.Mappers;
using Hng.Infrastructure.Repository.Interface;
using Hng.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;
using System.Linq.Expressions;
using Xunit;

namespace Hng.Application.Test.Features.Organization
{
    public class UpdateOrganizationShould
    {
        private readonly Mock<IRepository<Domain.Entities.Organization>> _repositoryMock;
        private readonly UpdateOrganizationCommandHandler _handler;

        public UpdateOrganizationShould()
        {
            var mappingProfile = new OrganizationMapperProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(mappingProfile));
            IMapper mapper = new Mapper(configuration);

            _repositoryMock = new Mock<IRepository<Domain.Entities.Organization>>();
            var authenticationServiceMock = new Mock<IAuthenticationService>();
            _handler = new UpdateOrganizationCommandHandler(_repositoryMock.Object, mapper, authenticationServiceMock.Object);

        }

        [Fact]
        public async Task Handle_ShouldReturn200_WhenOrganizationUpdatedSuccessfully()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var organizationId = Guid.NewGuid();
            var updateDto = new UpdateOrganizationDto
            {
                Name = "Updated Org",
                Description = "Updated Description",
                Email = "updated@example.com",
                Industry = "Tech",
                Type = "Enterprise",
                Country = "Updated Country",
                Address = "Updated Address",
                State = "Updated State"
            };

            var existingOrganization = new Domain.Entities.Organization
            {
                Id = organizationId,
                OwnerId = userId,
                Name = "Old Org",
                Description = "Old Description",
                Email = "old@example.com",
                Industry = "Old Industry",
                Type = "Old Type",
                Country = "Old Country",
                Address = "Old Address",
                State = "Old State"
            };

            _repositoryMock.Setup(r => r.AddAsync(It.IsAny<Domain.Entities.Organization>()))
                .ReturnsAsync((Domain.Entities.Organization org) =>
                {
                    org.Id = organizationId;
                    return org;
                });

            var command = new UpdateOrganizationCommand(organizationId, updateDto);

            // Act
            var result = await _handler.Handle(command, default);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
            Assert.Equal("Organization updated successfully", result.Message);
            Assert.NotNull(result.Data);
            Assert.Equal(updateDto.Name, result.Data.Name);
            Assert.Equal(updateDto.Description, result.Data.Description);
            Assert.Equal(updateDto.Email, result.Data.Email);
            Assert.Equal(updateDto.Industry, result.Data.Industry);
            Assert.Equal(updateDto.Type, result.Data.Type);
            Assert.Equal(updateDto.Country, result.Data.Country);
            Assert.Equal(updateDto.Address, result.Data.Address);
            Assert.Equal(updateDto.State, result.Data.State);
        }

        [Fact]
        public async Task Handle_ShouldReturn404_WhenOrganizationNotFound()
        {
            // Arrange
            var organizationId = Guid.NewGuid();
            var updateDto = new UpdateOrganizationDto { Name = "Updated Org" };
            var command = new UpdateOrganizationCommand(organizationId, updateDto);

            _repositoryMock.Setup(r => r.GetAsync(organizationId))
                .ReturnsAsync((Domain.Entities.Organization)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(404, result.StatusCode);
            Assert.Equal("Organization not found", result.Message);
        }

        [Fact]
        public async Task Handle_ShouldReturn403_WhenUnauthorizedUpdateAttempt()
        {
            // Arrange
            var organizationId = Guid.NewGuid();
            var ownerId = Guid.NewGuid();
            var unauthorizedUserId = Guid.NewGuid();
            var updateDto = new UpdateOrganizationDto { Name = "Updated Org" };
            var command = new UpdateOrganizationCommand(organizationId, updateDto);
            var organization = new Domain.Entities.Organization { Id = organizationId, OwnerId = ownerId };

            _repositoryMock.Setup(r => r.GetAsync(organizationId))
                .ReturnsAsync(organization);
            //_authenticationServiceMock.Setup(a => a.GetCurrentUserAsync())
            //    .ReturnsAsync(unauthorizedUserId);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(403, result.StatusCode);
            Assert.Equal("Unauthorized update attempt", result.Message);
        }

        [Fact]
        public async Task Handle_ShouldReturn400_WhenValidationErrorsOccur()
        {
            // Arrange
            var organizationId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var updateDto = new UpdateOrganizationDto { Name = "" }; // Invalid name
            var command = new UpdateOrganizationCommand(organizationId, updateDto);
            var organization = new Domain.Entities.Organization { Id = organizationId, OwnerId = userId };

            _repositoryMock.Setup(r => r.GetAsync(organizationId))
                .ReturnsAsync(organization);
            //_authenticationServiceMock.Setup(a => a.GetCurrentUserAsync())
            //    .ReturnsAsync(userId);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(400, result.StatusCode);
            Assert.Equal("Validation error", result.Message);
        }
    }

}
