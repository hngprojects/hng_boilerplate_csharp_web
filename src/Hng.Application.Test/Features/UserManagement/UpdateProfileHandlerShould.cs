﻿using AutoMapper;
using Hng.Application.Features.ExternalIntegrations.FilesUploadIntegrations.Cloudinary.Services;
using Hng.Application.Features.Profiles.Dtos;
using Hng.Application.Features.Profiles.Handlers;
using Hng.Application.Features.Profiles.Mappers;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using Moq;
using System.Linq.Expressions;
using Xunit;

namespace Hng.Application.Test.Features.UserManagement
{
    public class UpdateProfileHandlerShould
    {
        private readonly Mock<IRepository<User>> _userRepositoryMock;
        private readonly Mock<IRepository<Domain.Entities.Profile>> _profileRepositoryMock;
        private readonly Mock<IImageService> _imageServiceMock;
        private readonly IMapper _mapper;
        private readonly UpdateProfileHandler _handler;

        public UpdateProfileHandlerShould()
        {
            var profileMappingProfile = new ProfileMapperProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(profileMappingProfile));
            _mapper = new Mapper(configuration);

            _userRepositoryMock = new Mock<IRepository<User>>();
            _profileRepositoryMock = new Mock<IRepository<Domain.Entities.Profile>>();
            _imageServiceMock = new Mock<IImageService>();
            _handler = new UpdateProfileHandler(
                _userRepositoryMock.Object,
                _profileRepositoryMock.Object,
                _imageServiceMock.Object,
                _mapper);
        }

        [Fact]
        public async Task UpdateProfileHandlerShouldReturnProfileDto()
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
            var request = new UpdateProfileDto()
            { AvatarUrl = "https://res.cloudinary.com/kenelight4u/image/upload/v1723026364/HNG Bioler Plate/bmdqybm8pb2hu4dr8es7.jpg", Bio = "Good test" };

            _userRepositoryMock.Setup(repo => repo.GetBySpec(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<Expression<Func<User, object>>[]>()))
                .ReturnsAsync(user);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
        }
    }
}
