using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Hng.Application.Features.UserManagement.Commands;
using Hng.Application.Features.UserManagement.Dtos;
using Hng.Application.Features.UserManagement.Handlers;
using Hng.Domain.Entities;
using Hng.Domain.Enums;
using Hng.Infrastructure.Repository.Interface;
using Microsoft.AspNetCore.Http;
using Moq;
using Xunit;
using FluentAssertions;

public class UserActivationCommandShould
{
    private readonly Mock<IRepository<User>> _userRepositoryMock;
    private readonly UserActivationCommandHandler _handler;

    public UserActivationCommandShould()
    {
        _userRepositoryMock = new Mock<IRepository<User>>();
        _handler = new UserActivationCommandHandler(_userRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_UserNotFound_ReturnsNotFoundResponse()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var request = new UserActivationCommand { UserId = userId };

        _userRepositoryMock
            .Setup(repo => repo.GetBySpec(It.IsAny<Expression<Func<User, bool>>>()))
            .ReturnsAsync((User)null);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        result.Message.Should().Be("Organization not found.");
    }

    [Fact]
    public async Task Handle_UserAlreadyActive_ReturnsConflictResponse()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var request = new UserActivationCommand { UserId = userId };
        var user = new User { Id = userId, UserStatus = UserStatus.activate };

        _userRepositoryMock
            .Setup(repo => repo.GetBySpec(It.IsAny<Expression<Func<User, bool>>>()))
            .ReturnsAsync(user);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        result.Message.Should().Be("User Already Active.");
    }

    [Fact]
    public async Task Handle_ValidUser_ActivatesUserSuccessfully()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var request = new UserActivationCommand { UserId = userId };
        var user = new User { Id = userId, UserStatus = UserStatus.deactivate };

        _userRepositoryMock
            .Setup(repo => repo.GetBySpec(It.IsAny<Expression<Func<User, bool>>>()))
            .ReturnsAsync(user);

        _userRepositoryMock.Setup(repo => repo.UpdateAsync(It.IsAny<User>())).Returns(Task.CompletedTask);
        _userRepositoryMock.Setup(repo => repo.SaveChanges()).Returns(Task.CompletedTask);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.StatusCode.Should().Be(StatusCodes.Status200OK);
        result.Message.Should().Be("User activated successfully");

        _userRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<User>()), Times.Once);
        _userRepositoryMock.Verify(repo => repo.SaveChanges(), Times.Once);
    }
}
