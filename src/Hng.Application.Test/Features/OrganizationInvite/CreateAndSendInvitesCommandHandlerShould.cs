using Xunit;
using Moq;
using Hng.Application.Features.OrganisationInvite.Commands;
using Hng.Application.Features.OrganisationInvite.Dtos;
using Hng.Application.Features.OrganisationInvite.Handlers;
using Hng.Application.Features.OrganisationInvite.Validators;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using Hng.Infrastructure.Services.Interfaces;
using Hng.Infrastructure.Utilities;
using Microsoft.AspNetCore.Http;
using Hng.Application.Features.OrganisationInvite.Validators.ValidationErrors;

namespace Hng.Application.Tests.Features.OrganisationInvite.Handlers
{
    public class CreateAndSendInvitesCommandHandlerShould
    {
        private readonly Mock<IOrganisationInviteService> _mockInviteService;
        private readonly Mock<IMessageQueueService> _mockQueueService;
        private readonly Mock<IRepository<User>> _mockUserRepository;
        private readonly Mock<IRepository<Organization>> _mockOrganizationRepository;
        private readonly Mock<IRepository<OrganizationInvite>> _mockInviteRepository;
        private readonly Mock<IRequestValidator> _mockRequestValidator;
        private readonly CreateAndSendInvitesCommandHandler _handler;

        public CreateAndSendInvitesCommandHandlerShould()
        {
            _mockInviteService = new Mock<IOrganisationInviteService>();
            _mockQueueService = new Mock<IMessageQueueService>();
            _mockUserRepository = new Mock<IRepository<User>>();
            _mockOrganizationRepository = new Mock<IRepository<Organization>>();
            _mockInviteRepository = new Mock<IRepository<OrganizationInvite>>();
            _mockRequestValidator = new Mock<IRequestValidator>();

            _handler = new CreateAndSendInvitesCommandHandler(
                _mockInviteService.Object,
                _mockQueueService.Object,
                _mockUserRepository.Object,
                _mockOrganizationRepository.Object,
                _mockInviteRepository.Object,
                _mockRequestValidator.Object
            );
        }

        [Fact]
        public async Task Handle_WithValidRequest_ShouldReturnSuccessResponse()
        {
            // Arrange
            var orgId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var email = "test@example.com";
            var command = new CreateAndSendInvitesCommand(new CreateAndSendInvitesDto
            {
                OrgId = orgId.ToString(),
                InviterId = userId,
                Emails = new List<string> { email }
            });

            var organization = new Organization { Id = orgId, OwnerId = userId };
            var user = new User { Id = userId, FirstName = "John" };

            _mockRequestValidator.Setup(v => v.UserIsOrganizationOwnerAsync(userId, orgId, _mockOrganizationRepository.Object))
                .ReturnsAsync(Result<Organization>.Success(organization));
            _mockUserRepository.Setup(r => r.GetAsync(userId)).ReturnsAsync(user);
            _mockRequestValidator.Setup(v => v.InviteDoesNotExistAsync(orgId, email, _mockInviteRepository.Object))
                .ReturnsAsync(Result<OrganizationInvite>.Success(null));
            _mockInviteService.Setup(s => s.CreateInvite(userId, orgId, email))
                .ReturnsAsync(new OrganizationInvite { InviteLink = Guid.NewGuid() });

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
            Assert.Equal("Invitation(s) processed successfully!", result.Message);
            Assert.NotNull(result.Data);
        }

        [Fact]
        public async Task Handle_WithInvalidOrganization_ShouldReturnNotFoundResponse()
        {
            // Arrange
            var orgId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var email = "test@example.com";
            var command = new CreateAndSendInvitesCommand(new CreateAndSendInvitesDto
            {
                OrgId = orgId.ToString(),
                InviterId = userId,
                Emails = new List<string> { email }
            });

            _mockRequestValidator.Setup(v => v.UserIsOrganizationOwnerAsync(userId, orgId, _mockOrganizationRepository.Object))
                .ReturnsAsync(Result<Organization>.Failure(OrganisationDoesNotExistError.FromId(orgId)));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(StatusCodes.Status404NotFound, result.StatusCode);
            Assert.Contains("No organisation exists for the id", result.Message);
        }

        [Fact]
        public async Task Handle_WithUserNotOwner_ShouldReturnUnauthorizedResponse()
        {
            // Arrange
            var orgId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var email = "test@example.com";
            var command = new CreateAndSendInvitesCommand(new CreateAndSendInvitesDto
            {
                OrgId = orgId.ToString(),
                InviterId = userId,
                Emails = new List<string> { email }
            });

            _mockRequestValidator.Setup(v => v.UserIsOrganizationOwnerAsync(userId, orgId, _mockOrganizationRepository.Object))
                .ReturnsAsync(Result<Organization>.Failure(UserIsNotOwnerError.FromIds(userId, orgId)));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(StatusCodes.Status401Unauthorized, result.StatusCode);
            Assert.Contains("You are not the owner of the specified organization", result.Message);
        }

        [Fact]
        public async Task Handle_InviteAlreadyExists_ShouldReturnPartialSuccess()
        {
            // Arrange
            var orgId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var existingEmail = "existing@example.com";
            var newEmail = "new@example.com";
            var command = new CreateAndSendInvitesCommand(new CreateAndSendInvitesDto
            {
                OrgId = orgId.ToString(),
                InviterId = userId,
                Emails = new List<string> { existingEmail, newEmail }
            });

            var organization = new Organization { Id = orgId, OwnerId = userId };
            var user = new User { Id = userId, FirstName = "John" };

            _mockRequestValidator.Setup(v => v.UserIsOrganizationOwnerAsync(userId, orgId, _mockOrganizationRepository.Object))
                .ReturnsAsync(Result<Organization>.Success(organization));

            _mockUserRepository.Setup(r => r.GetAsync(userId)).ReturnsAsync(user);

            _mockRequestValidator.Setup(v => v.InviteDoesNotExistAsync(orgId, existingEmail, _mockInviteRepository.Object))
                .ReturnsAsync(Result<OrganizationInvite>.Failure(InviteAlreadyExistsError.FromEmail(existingEmail)));

            _mockRequestValidator.Setup(v => v.InviteDoesNotExistAsync(orgId, newEmail, _mockInviteRepository.Object))
                .ReturnsAsync(Result<OrganizationInvite>.Success(null));

            _mockInviteService.Setup(s => s.CreateInvite(userId, orgId, newEmail))
                .ReturnsAsync(new OrganizationInvite { InviteLink = Guid.NewGuid() });

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
            Assert.Equal("Invitation(s) processed successfully!", result.Message);
            var responseDto = Assert.IsType<CreateAndSendInvitesResponseDto>(result.Data);
            Assert.Equal(command.Details.Emails.Count(), responseDto.Invitations.Count());
            Assert.Contains(responseDto.Invitations, i => i.Email == existingEmail && i.Error != null);
            Assert.Contains(responseDto.Invitations, i => i.Email == newEmail && i.InviteLink != null);
        }
    }
}