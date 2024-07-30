using AutoMapper;
using Google.Apis.Auth;
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
    public class GoogleLoginCommandHandlerTests
    {
        private readonly Mock<IRepository<User>> _userRepoMock;
        private readonly Mock<ITokenService> _tokenServiceMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IGoogleAuthService> _googleAuthServiceMock;
        private readonly GoogleLoginCommandHandler _handler;

        public GoogleLoginCommandHandlerTests()
        {
            _userRepoMock = new Mock<IRepository<User>>();
            _tokenServiceMock = new Mock<ITokenService>();
            _mapperMock = new Mock<IMapper>();
            _googleAuthServiceMock = new Mock<IGoogleAuthService>();
            _handler = new GoogleLoginCommandHandler(
                _userRepoMock.Object,
                _tokenServiceMock.Object,
                _mapperMock.Object,
                _googleAuthServiceMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldRegisterNewUser_WhenUserDoesNotExist()
        {
            // Arrange
            var googleIdToken = "valid_id_token";
            var googlePayload = new GoogleJsonWebSignature.Payload
            {
                Email = "newuser@example.com",
                GivenName = "New",
                FamilyName = "User",
                Picture = "http://example.com/avatar.jpg"
            };

            var newUser = new User
            {
                Email = googlePayload.Email,
                FirstName = googlePayload.GivenName,
                LastName = googlePayload.FamilyName,
                AvatarUrl = googlePayload.Picture
            };

            var newUserDto = new UserDto
            {
                Email = googlePayload.Email
            };

            // Mock Google token validation
            _googleAuthServiceMock.Setup(service => service.ValidateAsync(It.IsAny<string>()))
                                  .ReturnsAsync(googlePayload);

            // Setup repository mocks
            _userRepoMock.Setup(repo => repo.GetBySpec(It.IsAny<Expression<Func<User, bool>>>()))
                         .ReturnsAsync((User)null);

            _userRepoMock.Setup(repo => repo.AddAsync(It.IsAny<User>()))
                         .ReturnsAsync(newUser); // Return newUser wrapped in a Task

            _mapperMock.Setup(m => m.Map<User>(It.IsAny<GoogleJsonWebSignature.Payload>()))
                       .Returns(newUser);

            _mapperMock.Setup(m => m.Map<UserDto>(It.IsAny<User>()))
                       .Returns(newUserDto);

            _tokenServiceMock.Setup(ts => ts.GenerateJwt(It.IsAny<User>()))
                             .Returns("fake_jwt_token");

            // Act
            var result = await _handler.Handle(new GoogleLoginCommand(googleIdToken), CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Registration successful, user logged in.", result.Message);
            Assert.NotNull(result.AccessToken);
            //Assert.Equal("newuser@example.com", result.Data.Email);
            _userRepoMock.Verify(repo => repo.AddAsync(It.Is<User>(u => u.Email == googlePayload.Email)), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldLoginExistingUser_WhenUserExists()
        {
            // Arrange
            var googleIdToken = "valid_id_token";
            var googlePayload = new GoogleJsonWebSignature.Payload
            {
                Email = "existinguser@example.com",
                GivenName = "Existing",
                FamilyName = "User",
            };

            var existingUser = new User
            {
                Email = googlePayload.Email,
                FirstName = googlePayload.GivenName,
                LastName = googlePayload.FamilyName,
            };

            var userDto = new UserDto
            {
                Email = googlePayload.Email
            };

            // Mock Google token validation
            _googleAuthServiceMock.Setup(service => service.ValidateAsync(It.IsAny<string>()))
                                  .ReturnsAsync(googlePayload);

            _userRepoMock.Setup(repo => repo.GetBySpec(It.IsAny<Expression<Func<User, bool>>>()))
                         .ReturnsAsync(existingUser);

            _mapperMock.Setup(m => m.Map<UserDto>(It.IsAny<User>()))
                       .Returns(userDto);

            _tokenServiceMock.Setup(ts => ts.GenerateJwt(It.IsAny<User>()))
                             .Returns("fake_jwt_token");

            // Act
            var result = await _handler.Handle(new GoogleLoginCommand(googleIdToken), CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Login successful", result.Message);
            Assert.NotNull(result.AccessToken);
            //Assert.Equal("existinguser@example.com", result.Data.user.Email);
            _userRepoMock.Verify(repo => repo.AddAsync(It.IsAny<User>()), Times.Never);
        }
    }
}
