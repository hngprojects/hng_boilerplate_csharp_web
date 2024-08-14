using Xunit;
using Moq;
using Hng.Application.Features.OrganisationInvite.Validators;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using Hng.Application.Features.OrganisationInvite.Validators.ValidationErrors;
using System.Linq.Expressions;

namespace Hng.Application.Tests.Features.OrganisationInvite.Validators
{
    public class RequestValidatorShould
    {
        private readonly RequestValidator _validator;
        private readonly Mock<IRepository<Organization>> _mockOrgRepository;
        private readonly Mock<IRepository<OrganizationInvite>> _mockInviteRepository;

        public RequestValidatorShould()
        {
            _validator = new RequestValidator();
            _mockOrgRepository = new Mock<IRepository<Organization>>();
            _mockInviteRepository = new Mock<IRepository<OrganizationInvite>>();
        }

        [Fact]
        public async Task OrganizationExistsAsync_WithExistingOrg_ShouldReturnSuccess()
        {
            // Arrange
            var orgId = Guid.NewGuid();
            var organization = new Organization { Id = orgId };
            _mockOrgRepository.Setup(r => r.GetAsync(orgId)).ReturnsAsync(organization);

            // Act
            var result = await _validator.OrganizationExistsAsync(orgId, _mockOrgRepository.Object);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(organization, result.Value);
        }

        [Fact]
        public async Task OrganizationExistsAsync_WithNonExistingOrg_ShouldReturnFailure()
        {
            // Arrange
            var orgId = Guid.NewGuid();
            _mockOrgRepository.Setup(r => r.GetAsync(orgId)).ReturnsAsync((Organization)null);

            // Act
            var result = await _validator.OrganizationExistsAsync(orgId, _mockOrgRepository.Object);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.IsType<OrganisationDoesNotExistError>(result.Error);
        }

        [Fact]
        public async Task UserIsOrganizationOwnerAsync_WithOwnerUser_ShouldReturnSuccess()
        {
            // Arrange
            var orgId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var organization = new Organization { Id = orgId, OwnerId = userId };
            _mockOrgRepository.Setup(r => r.GetAsync(orgId)).ReturnsAsync(organization);

            // Act
            var result = await _validator.UserIsOrganizationOwnerAsync(userId, orgId, _mockOrgRepository.Object);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(organization, result.Value);
        }

        [Fact]
        public async Task UserIsOrganizationOwnerAsync_NonOwnerUser_ShouldReturnFailure()
        {
            // Arrange
            var orgId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var ownerId = Guid.NewGuid();
            var organization = new Organization { Id = orgId, OwnerId = ownerId };
            _mockOrgRepository.Setup(r => r.GetAsync(orgId)).ReturnsAsync(organization);

            // Act
            var result = await _validator.UserIsOrganizationOwnerAsync(userId, orgId, _mockOrgRepository.Object);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.IsType<UserIsNotOwnerError>(result.Error);
        }

        [Fact]
        public async Task InviteDoesNotExistAsync_NonExistingInvite_ShouldReturnSuccess()
        {
            // Arrange
            var orgId = Guid.NewGuid();
            var email = "test@example.com";
            _mockInviteRepository.Setup(r => r.GetBySpec(It.IsAny<Expression<Func<OrganizationInvite, bool>>>()))
                .ReturnsAsync((OrganizationInvite)null);

            // Act
            var result = await _validator.InviteDoesNotExistAsync(orgId, email, _mockInviteRepository.Object);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Null(result.Value);
        }

        [Fact]
        public async Task InviteDoesNotExistAsync_ExistingInvite_ShouldReturnFailure()
        {
            // Arrange
            var orgId = Guid.NewGuid();
            var email = "test@example.com";
            var existingInvite = new OrganizationInvite { OrganizationId = orgId, Email = email };
            _mockInviteRepository.Setup(r => r.GetBySpec(It.IsAny<Expression<Func<OrganizationInvite, bool>>>()))
            .ReturnsAsync(existingInvite);

            // Act
            var result = await _validator.InviteDoesNotExistAsync(orgId, email, _mockInviteRepository.Object);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.IsType<InviteAlreadyExistsError>(result.Error);
        }
    }
}