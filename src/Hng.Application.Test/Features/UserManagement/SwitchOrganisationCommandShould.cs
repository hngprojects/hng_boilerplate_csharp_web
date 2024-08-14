using System.Linq.Expressions;
using Hng.Application.Features.UserManagement.Commands;
using Hng.Application.Features.UserManagement.Handlers;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using Hng.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Moq;
using Xunit;

namespace Hng.Application.Test.Features.UserManagement;

public class SwitchOrganisationCommandShould
{
    private readonly Mock<IRepository<User>> _userRepositoryMock;
    private readonly Mock<IRepository<Domain.Entities.Organization>> _organisationRepositoryMock;
    private readonly Mock<IAuthenticationService> _authenticationServiceMock;
    private readonly SwitchOrganisationCommandHandler _handler;

    public SwitchOrganisationCommandShould()
    {
        _userRepositoryMock = new Mock<IRepository<User>>();
        _organisationRepositoryMock = new Mock<IRepository<Domain.Entities.Organization>>();
        _authenticationServiceMock = new Mock<IAuthenticationService>();
        _handler = new SwitchOrganisationCommandHandler(
            _userRepositoryMock.Object,
            _organisationRepositoryMock.Object,
            _authenticationServiceMock.Object);
    }

    [Fact]
    public async Task Handle_UserIsNotMember_ReturnsUnauthorizedResponse()
    {
        // Arrange
        var command = new SwitchOrganisationCommand
        {
            OrganisationId = Guid.NewGuid(),
            IsActive = true
        };

        var organization = new Domain.Entities.Organization { Id = command.OrganisationId, Users = new List<User>() };
        var userId = Guid.NewGuid();
        _authenticationServiceMock.Setup(x => x.GetCurrentUserAsync()).ReturnsAsync(userId);
        _organisationRepositoryMock.Setup(x => x.GetBySpec(It.IsAny<Expression<Func<Domain.Entities.Organization, bool>>>()))
            .ReturnsAsync(organization);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal("Unauthorized request. You are not a member of this organisation.", result.Message);
        Assert.Equal(StatusCodes.Status403Forbidden, result.StatusCode);
    }

    [Fact]
    public async Task Handle_OrganisationAlreadyActive_ReturnsNoChangeResponse()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var command = new SwitchOrganisationCommand
        {
            OrganisationId = Guid.NewGuid(),
            IsActive = true
        };

        var organization = new Domain.Entities.Organization
        {
            Id = command.OrganisationId,
            Users = new List<User> { new User { Id = userId } }
        };
        var user = new User
        {
            Id = userId,
            Organizations = new List<Domain.Entities.Organization>
            {
                new Domain.Entities.Organization { Id = command.OrganisationId, IsActive = true }
            }
        };

        _authenticationServiceMock.Setup(x => x.GetCurrentUserAsync()).ReturnsAsync(userId);
        _organisationRepositoryMock.Setup(x => x.GetBySpec(It.IsAny<Expression<Func<Domain.Entities.Organization, bool>>>()))
            .ReturnsAsync(organization);
        _userRepositoryMock.Setup(x => x.GetBySpec(It.IsAny<Expression<Func<User, bool>>>()))
            .ReturnsAsync(user);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal("No change required. The organization is already active.", result.Message);
    }

    [Fact]
    public async Task Handle_OrganisationUpdatedSuccessfully_ReturnsSuccessResponse()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var command = new SwitchOrganisationCommand
        {
            OrganisationId = Guid.NewGuid(),
            IsActive = true
        };

        var organization = new Domain.Entities.Organization
        {
            Id = command.OrganisationId,
            Users = new List<User> { new User { Id = userId } }
        };
        var user = new User
        {
            Id = userId,
            Organizations = new List<Domain.Entities.Organization>
            {
                new Domain.Entities.Organization { Id = command.OrganisationId, IsActive = false }
            }
        };

        _authenticationServiceMock.Setup(x => x.GetCurrentUserAsync()).ReturnsAsync(userId);
        _organisationRepositoryMock.Setup(x => x.GetBySpec(It.IsAny<Expression<Func<Domain.Entities.Organization, bool>>>()))
            .ReturnsAsync(organization);
        _userRepositoryMock.Setup(x => x.GetBySpec(It.IsAny<Expression<Func<User, bool>>>()))
            .ReturnsAsync(user);
        _userRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<User>())).Returns(Task.CompletedTask);
        _userRepositoryMock.Setup(x => x.SaveChanges()).Returns(Task.CompletedTask);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal("Organisation successfully switched", result.Message);
        Assert.NotNull(result.OrganisationDto);
        Assert.Equal(command.OrganisationId, result.OrganisationDto.Id);
        Assert.True(result.OrganisationDto.IsActive);

        _userRepositoryMock.Verify(x => x.UpdateAsync(user), Times.Once);
        _userRepositoryMock.Verify(x => x.SaveChanges(), Times.Once);
    }
}