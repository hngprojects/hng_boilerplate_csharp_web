using AutoMapper;
using Hng.Application.Features.UserManagement.Commands;
using Hng.Application.Features.UserManagement.Dtos;
using Hng.Application.Features.UserManagement.Handlers;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using Hng.Infrastructure.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;
using System.Linq.Expressions;
using Xunit;

namespace Hng.Application.Test.Features.UserManagement
{
    public class UserSignUpCommandShould
    {
        private readonly IMapper _mapper;
        private readonly Mock<IRepository<User>> _userRepositoryMock;
        private readonly Mock<ILogger<UserSignUpCommandHandler>> _loggerMock;
        private readonly Mock<IPasswordService> _passwordServiceMock;
        private readonly Mock<ITokenService> _tokenServiceMock;

        public UserSignUpCommandShould()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<UserSignUpDto, User>();
                cfg.CreateMap<User, UserResponseDto>();
            });
            _mapper = config.CreateMapper();

            _userRepositoryMock = new Mock<IRepository<User>>();
            _loggerMock = new Mock<ILogger<UserSignUpCommandHandler>>();
            _passwordServiceMock = new Mock<IPasswordService>();
            _tokenServiceMock = new Mock<ITokenService>();
        }

        [Fact]
        public async Task ReturnSuccessResponseForValidSignUp()
        {
            // Arrange
            _userRepositoryMock.Setup(repo => repo.GetBySpec(It.IsAny<Expression<Func<User, bool>>>()))
                .ReturnsAsync((User)null);

            _passwordServiceMock.Setup(service => service.GeneratePasswordSaltAndHash(It.IsAny<string>()))
                .Returns(("hashedPassword", "salt"));

            _tokenServiceMock.Setup(service => service.GenerateJwt(It.IsAny<User>()))
                .Returns("token");

            var handler = new UserSignUpCommandHandler(
            _userRepositoryMock.Object,
            _mapper,
            _loggerMock.Object,
            _passwordServiceMock.Object,
            _tokenServiceMock.Object);


            var command = new UserSignUpCommand(new UserSignUpDto
            {
                Email = "newuser@example.com",
                Password = "password123",
                FirstName = "John",
                LastName = "Doe",
                PhoneNumber = "+2340987654321"
            });

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("User registered successfully", result.Message);
            Assert.NotNull(result.Data);
            Assert.Equal("token", result.Data.Token);
            Assert.NotNull(result.Data.User);
            Assert.Equal("newuser@example.com", result.Data.User.Email);

            _userRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<User>()), Times.Once);
            _userRepositoryMock.Verify(repo => repo.SaveChanges(), Times.Once);
        }

        [Fact]
        public async Task ReturnFailureResponseForNonUniqueEmail()
        {
            // Arrange
            var existingUser = new User { Email = "existing@example.com" };
            _userRepositoryMock.Setup(repo => repo.GetBySpec(It.IsAny<Expression<Func<User, bool>>>()))
                .ReturnsAsync(existingUser);

            var handler = new UserSignUpCommandHandler(
            _userRepositoryMock.Object,
            _mapper,
            _loggerMock.Object,
            _passwordServiceMock.Object,
            _tokenServiceMock.Object);

            var command = new UserSignUpCommand(new UserSignUpDto
            {
                Email = "existing@example.com",
                Password = "password123",
                FirstName = "John",
                LastName = "Doe",
                PhoneNumber = "+2340987654321"
            });

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Email already exists", result.Message);
            Assert.Null(result.Data);

            _userRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<User>()), Times.Never);
            _userRepositoryMock.Verify(repo => repo.SaveChanges(), Times.Never);
        }

        [Fact]
        public async Task HandleExceptionGracefully()
        {
            // Arrange
            _userRepositoryMock.Setup(repo => repo.GetBySpec(It.IsAny<Expression<Func<User, bool>>>()))
                .ThrowsAsync(new Exception("Database error"));

            var handler = new UserSignUpCommandHandler(
            _userRepositoryMock.Object,
            _mapper,
            _loggerMock.Object,
            _passwordServiceMock.Object,
            _tokenServiceMock.Object);

            var command = new UserSignUpCommand(new UserSignUpDto
            {
                Email = "newuser@example.com",
                Password = "password123",
                FirstName = "John",
                LastName = "Doe",
                PhoneNumber = "+2340987654321"
            });

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("An error occurred while processing your request", result.Message);
            Assert.Null(result.Data);
        }
    }
}