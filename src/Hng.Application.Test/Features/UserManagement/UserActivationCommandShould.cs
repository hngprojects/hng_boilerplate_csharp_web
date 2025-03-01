using AutoMapper;
using Hng.Application.Features.UserManagement.Commands;
using Hng.Application.Features.UserManagement.Dtos;
using Hng.Application.Features.UserManagement.Handlers;
using Hng.Domain.Entities;
using Hng.Domain.Enums;
using Hng.Infrastructure.Repository.Interface;
using Microsoft.AspNetCore.Http;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Hng.Application.Tests.Features.UserManagement.Handlers;

public class UserActivationCommandHandlerTests
{
    [Fact]
    public async Task Handle_UserNotFound_ReturnsNotFoundResponse()
    {
        // Arrange
        var mockUserRepository = new Mock<IRepository<User>>();
        var mockMapper = new Mock<IMapper>();
        var handler = new UserActivationCommandHandler(mockUserRepository.Object, mockMapper.Object);
        var command = new UserActivationCommand { UserId = Guid.NewGuid() };

        mockUserRepository.Setup(repo => repo.GetBySpec(It.IsAny<System.Linq.Expressions.Expression<Func<User, bool>>>()))
            .ReturnsAsync((User)null);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(StatusCodes.Status404NotFound, result.StatusCode);
        Assert.Equal("User not found.", result.Message);
    }

    [Fact]
    public async Task Handle_UserAlreadyActive_ReturnsBadRequestResponse()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = new User { Id = userId, UserStatus = UserStatus.activate };
        var mockUserRepository = new Mock<IRepository<User>>();
        var mockMapper = new Mock<IMapper>();
        var handler = new UserActivationCommandHandler(mockUserRepository.Object, mockMapper.Object);
        var command = new UserActivationCommand { UserId = userId };

        mockUserRepository.Setup(repo => repo.GetBySpec(It.IsAny<System.Linq.Expressions.Expression<Func<User, bool>>>()))
            .ReturnsAsync(user);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(StatusCodes.Status400BadRequest, result.StatusCode);
        Assert.Equal("User is already active.", result.Message);
    }

    [Fact]
    public async Task Handle_UserActivationSuccess_ReturnsOkResponse()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = new User { Id = userId, UserStatus = UserStatus.deactivate };
        var userDto = new UserResponseDto { Id = userId.ToString() };
        var mockUserRepository = new Mock<IRepository<User>>();
        var mockMapper = new Mock<IMapper>();
        var handler = new UserActivationCommandHandler(mockUserRepository.Object, mockMapper.Object);
        var command = new UserActivationCommand { UserId = userId };

        mockUserRepository.Setup(repo => repo.GetBySpec(It.IsAny<System.Linq.Expressions.Expression<Func<User, bool>>>()))
            .ReturnsAsync(user);
        mockUserRepository.Setup(repo => repo.UpdateAsync(It.IsAny<User>()))
            .Returns(Task.CompletedTask);
        mockUserRepository.Setup(repo => repo.SaveChanges())
            .Returns(Task.CompletedTask);
        mockMapper.Setup(mapper => mapper.Map<UserResponseDto>(It.IsAny<User>()))
            .Returns(userDto);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        Assert.Equal("User activated successfully.", result.Message);
    }

    [Fact]
    public async Task Handle_ExceptionThrown_ReturnsInternalServerError()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = new User { Id = userId, UserStatus = UserStatus.deactivate };
        var mockUserRepository = new Mock<IRepository<User>>();
        var mockMapper = new Mock<IMapper>();
        var handler = new UserActivationCommandHandler(mockUserRepository.Object, mockMapper.Object);
        var command = new UserActivationCommand { UserId = userId };

        mockUserRepository.Setup(repo => repo.GetBySpec(It.IsAny<System.Linq.Expressions.Expression<Func<User, bool>>>()))
            .ReturnsAsync(user);
        mockUserRepository.Setup(repo => repo.UpdateAsync(It.IsAny<User>()))
            .ThrowsAsync(new Exception("Database error"));

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(StatusCodes.Status500InternalServerError, result.StatusCode);
        Assert.Equal("An error occurred while activating the user.", result.Message);
    }
}