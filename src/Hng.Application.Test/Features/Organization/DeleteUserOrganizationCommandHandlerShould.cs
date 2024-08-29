using AutoMapper;
using Hng.Application.Features.Organisations.Commands;
using Hng.Application.Features.Organisations.Handlers;
using Hng.Infrastructure.Repository.Interface;
using Hng.Infrastructure.Services.Interfaces;
using Moq;
using System.Linq.Expressions;
using Xunit;

namespace Hng.Application.Test.Features.Organisations
{
    public class DeleteUserOrganizationCommandHandlerShould
    {
        private readonly Mock<IRepository<Domain.Entities.Organization>> _mockOrganizationRepository;
        private readonly Mock<IAuthenticationService> _mockAuthenticationService;
        private readonly Mock<IMapper> _mockMapper;
        private readonly DeleteUserOrganizationCommandHandler _handler;

        public DeleteUserOrganizationCommandHandlerShould()
        {
            _mockOrganizationRepository = new Mock<IRepository<Domain.Entities.Organization>>();
            _mockAuthenticationService = new Mock<IAuthenticationService>();
            _mockMapper = new Mock<IMapper>();
            _handler = new DeleteUserOrganizationCommandHandler(
                _mockOrganizationRepository.Object,
                _mockMapper.Object,
                _mockAuthenticationService.Object);
        }

        [Fact]
        public async Task Handle_ShouldDeleteOrganization_WhenOrganizationExists()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var organizationId = Guid.NewGuid();
            var command = new DeleteUserOrganizationCommand(organizationId);

            var organization = new Domain.Entities.Organization { Id = organizationId, OwnerId = userId };

            _mockAuthenticationService.Setup(a => a.GetCurrentUserAsync())
                .ReturnsAsync(userId);

            _mockOrganizationRepository.Setup(r => r.GetBySpec(It.IsAny<Expression<Func<Domain.Entities.Organization, bool>>>()))
                .ReturnsAsync(organization);

            _mockOrganizationRepository.Setup(r => r.DeleteAsync(It.IsAny<Domain.Entities.Organization>()));

            _mockOrganizationRepository.Setup(r => r.SaveChanges())
                .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result);
            _mockOrganizationRepository.Verify(r => r.DeleteAsync(It.Is<Domain.Entities.Organization>(o => o.Id == organizationId)), Times.Once);
            _mockOrganizationRepository.Verify(r => r.SaveChanges(), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnFalse_WhenOrganizationDoesNotExist()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var organizationId = Guid.NewGuid();
            var command = new DeleteUserOrganizationCommand(organizationId);

            _mockAuthenticationService.Setup(a => a.GetCurrentUserAsync())
                .ReturnsAsync(userId);

            _mockOrganizationRepository.Setup(r => r.GetBySpec(It.IsAny<Expression<Func<Domain.Entities.Organization, bool>>>()))
                .ReturnsAsync((Domain.Entities.Organization)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result);
            _mockOrganizationRepository.Verify(r => r.DeleteAsync(It.IsAny<Domain.Entities.Organization>()), Times.Never);
            _mockOrganizationRepository.Verify(r => r.SaveChanges(), Times.Never);
        }
    }
}
