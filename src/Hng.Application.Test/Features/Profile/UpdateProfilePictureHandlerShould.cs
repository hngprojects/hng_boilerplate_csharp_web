﻿using Hng.Application.Features.ExternalIntegrations.FilesUploadIntegrations.Cloudinary.Services;
using Hng.Application.Features.Profiles.Dtos;
using Hng.Application.Features.Profiles.Handlers;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using Hng.Infrastructure.Services.Interfaces;
using Moq;
using System.Linq.Expressions;
using Xunit;

namespace Hng.Application.Test.Features.Profile
{
    public class UpdateProfilePictureHandlerShould
    {
        private readonly Mock<IRepository<User>> _userRepositoryMock;
        private readonly Mock<IImageService> _imageServiceMock;
        private readonly Mock<IRepository<Domain.Entities.Profile>> _profileRepositoryMock;
        private readonly Mock<ITokenService> _tokenServiceMock;
        private readonly UpdateProfilePictureHandler _handler;

        public UpdateProfilePictureHandlerShould()
        {
            _userRepositoryMock = new Mock<IRepository<User>>();
            _imageServiceMock = new Mock<IImageService>();
            _profileRepositoryMock = new Mock<IRepository<Domain.Entities.Profile>>();
            _tokenServiceMock = new Mock<ITokenService>();
            _handler = new UpdateProfilePictureHandler(
                _userRepositoryMock.Object,
                _imageServiceMock.Object,
                _profileRepositoryMock.Object,
                _tokenServiceMock.Object);
        }

        [Fact]
        public async Task UpdateProfileHandlerShouldReturnSuccessDto()
        {
            // Arrange
            var userid = Guid.NewGuid();
            var user = new User()
            {
                Email = "ken@yopmail",
                Password = "^tghdt%$re#$$3",
                PasswordSalt = "$$%$%ffddff-78yh--hhgg",
                Id = userid,
                Profile = new Domain.Entities.Profile() { UserId = userid, Id = Guid.NewGuid() }
            };
            var userProfile = user.Profile;

            var request = new UpdateProfilePictureDto() { };

            _tokenServiceMock.Setup(f => f.GetCurrentUserEmail()).Returns(user.Email);
            _userRepositoryMock.Setup(repo => repo.GetBySpec(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<Expression<Func<User, object>>[]>()))
                .ReturnsAsync(user);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
        }
    }
}
