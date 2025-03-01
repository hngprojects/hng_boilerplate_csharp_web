using AutoMapper;
using Hng.Application.Features.Organisations.Commands;
using Hng.Application.Features.Organisations.Dtos;
using Hng.Application.Features.Organisations.Handlers;
using Hng.Application.Features.Organisations.Mappers;
using Hng.Infrastructure.Repository.Interface;
using Hng.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Moq;
using System.Linq.Expressions;
using Xunit;

namespace Hng.Application.Test.Features.Organization
{
    public class UpdateOrganizationShould
    {
        private readonly Mock<IRepository<Domain.Entities.Organization>> _repositoryMock;
        private readonly Mock<IAuthenticationService> _authenticationServiceMock;
        private readonly UpdateOrganizationCommandHandler _handler;
        private readonly IMapper _mapper;

        public UpdateOrganizationShould()
        {
            // Setup mapper with proper profile
            var mappingProfile = new OrganizationMapperProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(mappingProfile));
            _mapper = new Mapper(configuration);

            // Setup repository and authentication mocks
            _repositoryMock = new Mock<IRepository<Domain.Entities.Organization>>();
            _authenticationServiceMock = new Mock<IAuthenticationService>();

            // Initialize the handler with mocks
            _handler = new UpdateOrganizationCommandHandler(
                _repositoryMock.Object,
                _mapper,
                _authenticationServiceMock.Object
            );
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

            // Setup authentication to return the current user
            _authenticationServiceMock
                .Setup(a => a.GetCurrentUserAsync())
                .ReturnsAsync(userId);

            // Setup repository to return existing organization when GetBySpec is called
            _repositoryMock
                .Setup(r => r.GetBySpec(It.IsAny<Expression<Func<Domain.Entities.Organization, bool>>>()))
                .ReturnsAsync(existingOrganization);

            // Setup UpdateAsync to do nothing (void return)
            _repositoryMock
                .Setup(r => r.UpdateAsync(It.IsAny<Domain.Entities.Organization>()))
                .Returns(Task.CompletedTask);

            // Setup SaveChanges to return success
            _repositoryMock
                .Setup(r => r.SaveChanges())
                .Returns(Task.FromResult(true));

            var command = new UpdateOrganizationCommand(organizationId, updateDto);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
            Assert.Equal("Organization updated successfully", result.Message);
            Assert.NotNull(result.Data);

            // Verify all properties were mapped correctly
            Assert.Equal(updateDto.Name, result.Data.Name);
            Assert.Equal(updateDto.Description, result.Data.Description);
            Assert.Equal(updateDto.Email, result.Data.Email);
            Assert.Equal(updateDto.Industry, result.Data.Industry);
            Assert.Equal(updateDto.Type, result.Data.Type);
            Assert.Equal(updateDto.Country, result.Data.Country);
            Assert.Equal(updateDto.Address, result.Data.Address);
            Assert.Equal(updateDto.State, result.Data.State);

            // Verify repository methods were called
            _repositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Domain.Entities.Organization>()), Times.Once);
            _repositoryMock.Verify(r => r.SaveChanges(), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturn404_WhenOrganizationNotFound()
        {
            // Arrange
            var organizationId = Guid.NewGuid();
            var updateDto = new UpdateOrganizationDto { Name = "Updated Org" };
            var command = new UpdateOrganizationCommand(organizationId, updateDto);

            // Setup repository to return null (organization not found)
            _repositoryMock
                .Setup(r => r.GetBySpec(It.IsAny<Expression<Func<Domain.Entities.Organization, bool>>>()))
                .ReturnsAsync((Domain.Entities.Organization)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(StatusCodes.Status404NotFound, result.StatusCode);
            Assert.Equal("Organization not found", result.Message);

            // Verify Update/Save were never called
            _repositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Domain.Entities.Organization>()), Times.Never);
            _repositoryMock.Verify(r => r.SaveChanges(), Times.Never);
        }

        [Fact]
        public async Task Handle_ShouldReturn403_WhenUnauthorizedUpdateAttempt()
        {
            // Arrange
            var organizationId = Guid.NewGuid();
            var ownerId = Guid.NewGuid();
            var unauthorizedUserId = Guid.NewGuid(); // Different from owner
            var updateDto = new UpdateOrganizationDto { Name = "Updated Org" };
            var command = new UpdateOrganizationCommand(organizationId, updateDto);

            var organization = new Domain.Entities.Organization
            {
                Id = organizationId,
                OwnerId = ownerId  // This is the owner
            };

            // Setup authentication to return unauthorized user
            _authenticationServiceMock
                .Setup(a => a.GetCurrentUserAsync())
                .ReturnsAsync(unauthorizedUserId);  // Different from owner

            // Setup repository to return the organization
            _repositoryMock
                .Setup(r => r.GetBySpec(It.IsAny<Expression<Func<Domain.Entities.Organization, bool>>>()))
                .ReturnsAsync(organization);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(StatusCodes.Status403Forbidden, result.StatusCode);
            Assert.Equal("You are not authorized to update this organization", result.Message);

            // Verify Update/Save were never called
            _repositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Domain.Entities.Organization>()), Times.Never);
            _repositoryMock.Verify(r => r.SaveChanges(), Times.Never);
        }
    }

}
