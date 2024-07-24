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
    public class LoginUserQueryShould
    {
        private readonly IMapper _mapper;

        public LoginUserQueryShould()
        {

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<User, UserDto>();
            });
            _mapper = config.CreateMapper();
        }
        [Fact]
        public async Task ReturnLoginResponseDtoForValidCredentials()
        {
            // Arrange
            var user = new User
            {
                Id = Guid.NewGuid(),
                Email = "test@example.com",
                FirstName = "John",
                LastName = "Doe",
                Password = "hashedpassword",
                PasswordSalt = "salt"
            };

            var userRepositoryMock = new Mock<IRepository<User>>();
            userRepositoryMock.Setup(repo => repo.GetBySpec(It.IsAny<Expression<Func<User, bool>>>()))
                .ReturnsAsync(user);

            var passwordServiceMock = new Mock<IPasswordService>();
            passwordServiceMock.Setup(service => service.IsPasswordEqual("password", user.PasswordSalt, user.Password))
                .Returns(true);

            var tokenServiceMock = new Mock<ITokenService>();
            tokenServiceMock.Setup(service => service.GenerateJwt(It.IsAny<User>()))
                .Returns("token");

            var handler = new LoginUserCommandHandler(userRepositoryMock.Object, _mapper, passwordServiceMock.Object, tokenServiceMock.Object);

            var command = new CreateUserLoginCommand(new UserLoginRequestDto
            {
                Email = "test@example.com",
                Password = "password"
            });

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            //Assert.Equal("Login successful", result.Message);
            Assert.Equal("token", result.AccessToken);
            Assert.NotNull(result.Data);
            Assert.Equal(user.Email, result.Data.Email);
        }


        [Fact]
        public async Task ReturnNullForInvalidCredentials()
        {
            // Arrange
            var user = new User
            {
                Id = Guid.NewGuid(),
                Email = "test@example.com",
                FirstName = "John",
                LastName = "Doe",
                Password = "hashedpassword",
                PasswordSalt = "salt"
            };

            var userRepositoryMock = new Mock<IRepository<User>>();
            userRepositoryMock.Setup(repo => repo.GetBySpec(It.IsAny<Expression<Func<User, bool>>>()))
                .ReturnsAsync(user);

            var passwordServiceMock = new Mock<IPasswordService>();
            passwordServiceMock.Setup(service => service.IsPasswordEqual("invalidpassword", user.PasswordSalt, user.Password))
                .Returns(false);

            var tokenServiceMock = new Mock<ITokenService>();

            var handler = new LoginUserCommandHandler(userRepositoryMock.Object, _mapper, passwordServiceMock.Object, tokenServiceMock.Object);

            var command = new CreateUserLoginCommand(new UserLoginRequestDto
            {
                Email = "test@example.com",
                Password = "invalidpassword"
            });

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Null(result);
           
        }

        [Fact]
        public async Task ReturnNullForNonExistentUser()
        {
            // Arrange
            var userRepositoryMock = new Mock<IRepository<User>>();
            userRepositoryMock.Setup(repo => repo.GetBySpec(It.IsAny<Expression<Func<User, bool>>>()))
                .ReturnsAsync((User)null);

            var passwordServiceMock = new Mock<IPasswordService>();

            var tokenServiceMock = new Mock<ITokenService>();

            var handler = new LoginUserCommandHandler(userRepositoryMock.Object, _mapper, passwordServiceMock.Object, tokenServiceMock.Object);

            var command = new CreateUserLoginCommand(new UserLoginRequestDto
            {
                Email = "nonexistent@example.com",
                Password = "password"
            });

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Null(result);
        }

    }
}
