using AutoMapper;
using Google.Apis.Auth;
using Hng.Application.Features.UserManagement.Commands;
using Hng.Application.Features.UserManagement.Dtos;
using Hng.Application.Features.UserManagement.Handlers;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using Hng.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Http;
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
    public class GoogleLoginCommandHandlerTests
    {
        private readonly Mock<IRepository<User>> _mockUserRepo;
        private readonly Mock<IRepository<Role>> _mockRoleRepo;
        private readonly Mock<ITokenService> _mockTokenService;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IGoogleAuthService> _mockGoogleAuthService;
        private readonly GoogleLoginCommandHandler _handler;

        public GoogleLoginCommandHandlerTests()
        {
            _mockUserRepo = new Mock<IRepository<User>>();
            _mockRoleRepo = new Mock<IRepository<Role>>();
            _mockTokenService = new Mock<ITokenService>();
            _mockMapper = new Mock<IMapper>();
            _mockGoogleAuthService = new Mock<IGoogleAuthService>();

            _handler = new GoogleLoginCommandHandler(
                _mockUserRepo.Object,
                _mockRoleRepo.Object,
                _mockTokenService.Object,
                _mockMapper.Object,
                _mockGoogleAuthService.Object
            );
        }

        [Fact]
        public async Task Handle_GivenInvalidGoogleToken_ShouldReturnUnauthorizedResponse()
        {
            // Arrange
            var request = new GoogleLoginCommand("valid-token");

            _mockGoogleAuthService
                .Setup(x => x.ValidateAsync(request.IdToken))
                .ThrowsAsync(new InvalidJwtException("Invalid token"));

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.Equal(StatusCodes.Status401Unauthorized, result.StatusCode);
            Assert.Null(result.Data);
            Assert.Equal("Invalid Google token.", result.Message);
        }

        [Fact]
        public async Task Handle_GivenValidGoogleTokenAndNewUser_ShouldRegisterUserAndReturnSuccessResponse()
        {
            // Arrange
            var request = new GoogleLoginCommand("valid-token");

            var payload = new GoogleJsonWebSignature.Payload
            {
                Email = "newuser@example.com",
                Name = "New User",
                Picture = "https://example.com/avatar.jpg"
            };

            _mockGoogleAuthService
                .Setup(x => x.ValidateAsync(request.IdToken))
                .ReturnsAsync(payload);

            _mockUserRepo
                   .Setup(x => x.GetBySpec(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<Expression<Func<User, object>>>()))
                   .ReturnsAsync((User)null);

            var newUser = new User
            {
                Email = payload.Email,
                FirstName = "New",
                LastName = "User",
                AvatarUrl = payload.Picture,
                Organizations = new List<Domain.Entities.Organization>()
            };

            _mockMapper.Setup(x => x.Map<User>(payload)).Returns(newUser);
            _mockTokenService.Setup(x => x.GenerateJwt(It.IsAny<User>())).Returns("token");

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            _mockUserRepo.Verify(x => x.AddAsync(It.IsAny<User>()), Times.Once);
            _mockRoleRepo.Verify(x => x.AddAsync(It.IsAny<Role>()), Times.Once);
            _mockUserRepo.Verify(x => x.SaveChanges(), Times.Once);
            Assert.Equal("Registration successful, user logged in.", result.Message);
            Assert.NotNull(result.Data);
            Assert.Equal("token", result.AccessToken);
        }

        [Fact]
        public async Task Handle_GivenValidGoogleTokenAndExistingUser_ShouldReturnSuccessResponse()
        {
            // Arrange
            var request = new GoogleLoginCommand("valid-token");

            var payload = new GoogleJsonWebSignature.Payload
            {
                Email = "existinguser@example.com",
                Name = "Existing User",
                Picture = "https://example.com/avatar.jpg"
            };

            _mockGoogleAuthService
                .Setup(x => x.ValidateAsync(request.IdToken))
                .ReturnsAsync(payload);

            var existingUser = new User
            {
                Email = payload.Email,
                FirstName = "Existing",
                LastName = "User",
                AvatarUrl = payload.Picture,
                Organizations = new List<Domain.Entities.Organization>
                {
                    new Domain.Entities.Organization { Id = Guid.NewGuid(), Name = "Org1", OwnerId = Guid.NewGuid() }
                }
            };

            _mockUserRepo
                .Setup(x => x.GetBySpec(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<Expression<Func<User, object>>>()))
                .ReturnsAsync(existingUser);

            _mockTokenService.Setup(x => x.GenerateJwt(It.IsAny<User>())).Returns("token");

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.Equal("Login successful", result.Message);
            Assert.NotNull(result.Data);
            Assert.Equal("token", result.AccessToken);
        }

    }

}
