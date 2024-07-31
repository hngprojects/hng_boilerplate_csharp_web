using AutoMapper;
using Hng.Application.Features.UserManagement.Commands;
using Hng.Application.Features.UserManagement.Handlers;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using Hng.Infrastructure.Services.Interfaces;
using Microsoft.Extensions.Logging;
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
        public async Task Handle_ShouldReturnSuccessResponse_WhenFacebookTokenIsValid()
        {
            // Arrange
            var facebookUser = new FacebookUser
            {
                Id = "facebook-id",
                Name = "John Doe",
                Email = "john.doe@example.com"
            };

            var user = new User
            {
                Id = Guid.NewGuid(),
                Email = facebookUser.Email,
                FirstName = facebookUser.Name
            };

            var token = "jwt-token";
            var request = new FacebookLoginCommand("valid-token");

            _facebookAuthServiceMock
                .Setup(service => service.ValidateAsync(request.FacebookToken))
                .ReturnsAsync(facebookUser);

            _userRepoMock
                .Setup(repo => repo.GetBySpec(It.IsAny<Expression<Func<User, bool>>>()))
                .ReturnsAsync(user);

            _tokenServiceMock
                .Setup(service => service.GenerateJwt(user))
                .Returns(token);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Login successful.", result.Message);
            Assert.Equal(user.Id, result.Data.Data.Id);
            Assert.Equal(user.FirstName, result.Data.Data.FullName);
            Assert.Equal(token, result.Data.AccessToken);
        }


        [Fact]
        public async Task Handle_ShouldCreateUser_WhenFacebookUserIsNotFoundInRepo()
        {
            // Arrange
            var facebookUser = new FacebookUser
            {
                Id = "facebook-id",
                Name = "John Doe",
                Email = "john.doe@example.com"
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
                .Setup(service => service.GenerateJwt(It.IsAny<User>()))
                .Returns("jwt-token");

            _mapperMock
                .Setup(m => m.Map<User>(It.IsAny<FacebookUser>()))
                .Returns(new User
                {
                    FirstName = facebookUser.Name,
                    Email = facebookUser.Email
                });

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            _userRepoMock.Verify(repo => repo.AddAsync(It.Is<User>(u => u.Email == facebookUser.Email && u.FirstName == facebookUser.Name)), Times.Once);
            _userRepoMock.Verify(repo => repo.SaveChanges(), Times.Once);
            Assert.Equal("Login successful.", result.Message);
        }





        [Fact]
        public async Task Handle_ShouldThrowException_WhenFacebookTokenIsInvalid()
        {
            // Arrange
            var request = new FacebookLoginCommand("valid-token");


            _facebookAuthServiceMock
                .Setup(service => service.ValidateAsync(request.FacebookToken))
                .ReturnsAsync((FacebookUser)null);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _handler.Handle(request, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_ShouldLogError_WhenExceptionOccurs()
        {
            // Arrange
            var request = new FacebookLoginCommand("valid-token");


            _facebookAuthServiceMock
                .Setup(service => service.ValidateAsync(request.FacebookToken))
                .ThrowsAsync(new Exception("Service error"));

            // Capture logged messages
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
            var exception = await Assert.ThrowsAsync<Exception>(() => _handler.Handle(request, CancellationToken.None));

            // Assert
            Assert.Equal("Login failed.", exception.Message);

            // Verify that Log method was called with LogLevel.Error
            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => true),
                    It.IsAny<Exception>(),
                    (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()),
                Times.Once);

            // Check if the error message contains specific text
            Assert.Contains(loggedMessages, msg => msg.Contains("Facebook login failed:"));
        }


    }

}
