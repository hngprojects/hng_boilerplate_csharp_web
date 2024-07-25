using AutoMapper;
using Hng.Application.Features.UserManagement.Commands;
using Hng.Application.Features.UserManagement.Dtos;
using Hng.Application.Features.UserManagement.Handlers;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using Hng.Infrastructure.Services.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Hng.Application.Test.Features.UserManagement
{
    public class LoginUserCommandShould
    {
        private readonly IMapper _mapper;
        private readonly Mock<IRepository<User>> _userRepositoryMock;
        private readonly Mock<IPasswordService> _passwordServiceMock;
        private readonly Mock<ITokenService> _tokenServiceMock;
        private readonly User _user;

        public LoginUserCommandShould()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<User, UserDto>();
            });
            _mapper = config.CreateMapper();

            _userRepositoryMock = new Mock<IRepository<User>>();
            _passwordServiceMock = new Mock<IPasswordService>();
            _tokenServiceMock = new Mock<ITokenService>();

            _user = new User
            {
                Id = Guid.NewGuid(),
                Email = "test@example.com",
                FirstName = "John",
                LastName = "Doe",
                Password = "hashedpassword",
                PasswordSalt = "salt"
            };
        }

        [Fact]
        public async Task ReturnLoginResponseDtoForValidCredentials()
        {
            // Arrange
            _userRepositoryMock.Setup(repo => repo.GetBySpec(It.IsAny<Expression<Func<User, bool>>>()))
                .ReturnsAsync(_user);

            _passwordServiceMock.Setup(service => service.IsPasswordEqual("password", _user.PasswordSalt, _user.Password))
                .Returns(true);

            _tokenServiceMock.Setup(service => service.GenerateJwt(It.IsAny<User>()))
                .Returns("token");

            var handler = new LoginUserCommandHandler(_userRepositoryMock.Object, _mapper, _passwordServiceMock.Object, _tokenServiceMock.Object);

            var command = new CreateUserLoginCommand(new UserLoginRequestDto
            {
                Email = "test@example.com",
                Password = "password"
            });

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Login successful", result.Message);
            Assert.Equal("token", result.AccessToken);
            Assert.NotNull(result.Data);
            Assert.Equal(_user.Email, result.Data.Email);
        }

        [Fact]
        public async Task ReturnNullForInvalidCredentials()
        {
            // Arrange
            _userRepositoryMock.Setup(repo => repo.GetBySpec(It.IsAny<Expression<Func<User, bool>>>()))
                .ReturnsAsync(_user);

            _passwordServiceMock.Setup(service => service.IsPasswordEqual("invalidpassword", _user.PasswordSalt, _user.Password))
                .Returns(false);

            var handler = new LoginUserCommandHandler(_userRepositoryMock.Object, _mapper, _passwordServiceMock.Object, _tokenServiceMock.Object);

            var command = new CreateUserLoginCommand(new UserLoginRequestDto
            {
                Email = "test@example.com",
                Password = "invalidpassword"
            });

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Null(result.Data);
            Assert.Null(result.AccessToken);
            Assert.Equal("Invalid credentials", result.Message);
        }

        [Fact]
        public async Task ReturnNullForNonExistentUser()
        {
            // Arrange
            _userRepositoryMock.Setup(repo => repo.GetBySpec(It.IsAny<Expression<Func<User, bool>>>()))
                .ReturnsAsync((User)null);

            var handler = new LoginUserCommandHandler(_userRepositoryMock.Object, _mapper, _passwordServiceMock.Object, _tokenServiceMock.Object);

            var command = new CreateUserLoginCommand(new UserLoginRequestDto
            {
                Email = "nonexistent@example.com",
                Password = "password"
            });

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Null(result.Data);
            Assert.Null(result.AccessToken);
            Assert.Equal("Invalid credentials", result.Message);
        }
    }

}
