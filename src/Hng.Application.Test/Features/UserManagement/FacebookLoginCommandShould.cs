using AutoMapper;
using Hng.Application.Features.UserManagement.Commands;
using Hng.Application.Features.UserManagement.Dtos;
using Hng.Application.Features.UserManagement.Handlers;
using Hng.Application.Shared.Dtos;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using Hng.Infrastructure.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Hng.Application.Test.Features.UserManagement
{
    public class FacebookLoginCommandShould
    {
        private readonly Mock<IRepository<User>> _userRepoMock;
        private readonly Mock<ITokenService> _tokenServiceMock;
        private readonly Mock<IFacebookAuthService> _facebookAuthServiceMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILogger<FacebookLoginCommandHandler>> _loggerMock;
        private readonly FacebookLoginCommandHandler _handler;

        public FacebookLoginCommandShould()
        {
            _userRepoMock = new Mock<IRepository<User>>();
            _tokenServiceMock = new Mock<ITokenService>();
            _facebookAuthServiceMock = new Mock<IFacebookAuthService>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<FacebookLoginCommandHandler>>();
            _handler = new FacebookLoginCommandHandler(
                _userRepoMock.Object,
                _tokenServiceMock.Object,
                _facebookAuthServiceMock.Object,
                _mapperMock.Object,
                _loggerMock.Object
            );
        }

        [Fact]
        public async Task Handle_ShouldLoginExistingUser_WhenFacebookTokenIsValid()
        {
            // Arrange
            var facebookToken = "valid_token";
            var facebookUser = new FacebookUser
            {
                Email = "existinguser@example.com",
                Name = "Existing",
                Picture = new FacebookPicture
                {
                    Data = new FacebookPictureData { Url = "http://example.com/avatar.jpg" }
                }
            };

            var existingUser = new User
            {
                Email = facebookUser.Email,
                FirstName = facebookUser.Name,
                AvatarUrl = facebookUser.Picture.Data.Url
            };

            var userDto = new UserDto
            {
                Email = facebookUser.Email,
                FullName = $"{facebookUser.Name}"
            };

            // Mock Facebook token validation
            _facebookAuthServiceMock.Setup(service => service.ValidateAsync(It.IsAny<string>()))
                                    .ReturnsAsync(facebookUser);

            _userRepoMock.Setup(repo => repo.GetBySpec(It.IsAny<Expression<Func<User, bool>>>()))
                         .ReturnsAsync(existingUser);

            _mapperMock.Setup(m => m.Map<UserDto>(It.IsAny<User>()))
                       .Returns(userDto);

            _tokenServiceMock.Setup(ts => ts.GenerateJwt(It.IsAny<User>(), It.IsAny<int>()))
                             .Returns("fake_jwt_token");

            // Act
            var result = await _handler.Handle(new FacebookLoginCommand(facebookToken), CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Login successful", result.Message);
            Assert.NotNull(result.AccessToken);

            var resultData = result.Data;
            Assert.NotNull(resultData);

            // Use reflection to access properties in the anonymous object
            var userProperty = resultData.GetType().GetProperty("user");
            var accessTokenProperty = resultData.GetType().GetProperty("access_token");

            Assert.NotNull(userProperty);
            Assert.NotNull(accessTokenProperty);

            var userData = userProperty.GetValue(resultData);
            var accessToken = accessTokenProperty.GetValue(resultData);

            Assert.Equal(userDto.Email, userDto.Email);
            Assert.Equal(userDto.FullName, userDto.FullName);
            Assert.Equal("fake_jwt_token", accessToken);

            _userRepoMock.Verify(repo => repo.AddAsync(It.IsAny<User>()), Times.Never);
        }


        [Fact]
        public async Task Handle_ShouldCreateUser_WhenFacebookUserIsNotFoundInRepo()
        {
            // Arrange
            var facebookUser = new FacebookUser
            {
                Id = "facebook-id",
                Name = "John Doe",
                Email = "john.doe@example.com",
                Picture = new FacebookPicture
                {
                    Data = new FacebookPictureData
                    {
                        Url = "https://example.com/picture.jpg"
                    }
                }
            };

            var request = new FacebookLoginCommand("valid-token");

            _facebookAuthServiceMock
                .Setup(service => service.ValidateAsync(request.FacebookToken))
                .ReturnsAsync(facebookUser);

            _userRepoMock
                .Setup(repo => repo.GetBySpec(It.IsAny<Expression<Func<User, bool>>>()))
                .ReturnsAsync((User)null);

            _userRepoMock
                .Setup(repo => repo.AddAsync(It.IsAny<User>()))
                .ReturnsAsync((User user) => user)
                .Verifiable();

            _userRepoMock
                .Setup(repo => repo.SaveChanges())
                .Returns(Task.CompletedTask)
                .Verifiable();

            _tokenServiceMock
                .Setup(service => service.GenerateJwt(It.IsAny<User>(), It.IsAny<int>()))
                .Returns("jwt-token");

            _mapperMock
                .Setup(m => m.Map<User>(It.IsAny<FacebookUser>()))
                .Returns(new User
                {
                    FirstName = facebookUser.Name,
                    Email = facebookUser.Email,
                    AvatarUrl = facebookUser.Picture.Data.Url
                });

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            _userRepoMock.Verify(repo => repo.AddAsync(It.Is<User>(u => u.Email == facebookUser.Email && u.FirstName == facebookUser.Name)), Times.Once);
            _userRepoMock.Verify(repo => repo.SaveChanges(), Times.Once);
            Assert.Equal("Registration successful, user logged in.", result.Message);
        }

        [Fact]
        public async Task Handle_ShouldReturnErrorResponse_WhenFacebookTokenIsInvalid()
        {
            // Arrange
            var request = new FacebookLoginCommand("invalid-token");

            _facebookAuthServiceMock
                .Setup(service => service.ValidateAsync(request.FacebookToken))
                .ReturnsAsync((FacebookUser)null);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Invalid Facebook token.", result.Message);
            Assert.Null(result.Data);
        }

        [Fact]
        public async Task Handle_ShouldLogError_WhenExceptionOccurs()
        {
            // Arrange
            var request = new FacebookLoginCommand("valid-token");

            _facebookAuthServiceMock
                .Setup(service => service.ValidateAsync(request.FacebookToken))
                .ThrowsAsync(new Exception("Service error"));

            var loggedMessages = new List<string>();
            _loggerMock
                .Setup(x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => true),
                    It.IsAny<Exception>(),
                    (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()))
                .Callback(new InvocationAction(i =>
                {
                    var state = (IReadOnlyList<KeyValuePair<string, object>>)i.Arguments[2];
                    loggedMessages.Add(state.FirstOrDefault(pair => pair.Key == "{OriginalFormat}").Value.ToString());
                }));

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.Equal("Login failed.", result.Message);
            Assert.Null(result.Data);

            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => true),
                    It.IsAny<Exception>(),
                    (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()),
                Times.Once);

            Assert.Contains(loggedMessages, msg => msg.Contains("Facebook login failed:"));
        }
    }
}
