using AutoMapper;
using Google.Apis.Auth;
using Hng.Application.Features.UserManagement.Dtos;
using Hng.Application.Features.UserManagement.Handlers;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using Hng.Infrastructure.Services.Interfaces;
using Moq;

namespace Hng.Application.Test.Features.UserManagement
{
    public class GoogleLoginCommandHandlerTests
    {
        private readonly Mock<IRepository<User>> _userRepoMock;
        private readonly Mock<IRepository<Role>> _roleRepoMock;
        private readonly Mock<ITokenService> _tokenServiceMock;
        private readonly IMapper _mapper;
        private readonly Mock<IGoogleAuthService> _googleAuthServiceMock;
        private readonly GoogleLoginCommandHandler _handler;

        public GoogleLoginCommandHandlerTests()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<User, UserDto>();
                cfg.CreateMap<User, UserResponseDto>();
                cfg.CreateMap<User, GoogleJsonWebSignature.Payload>().ReverseMap();
            });
            _mapper = config.CreateMapper();

            _userRepoMock = new Mock<IRepository<User>>();
            _roleRepoMock = new Mock<IRepository<Role>>();
            _tokenServiceMock = new Mock<ITokenService>();
            _googleAuthServiceMock = new Mock<IGoogleAuthService>();
            _handler = new GoogleLoginCommandHandler(
                _userRepoMock.Object,
                _roleRepoMock.Object,
                _tokenServiceMock.Object,
                _mapper,
                _googleAuthServiceMock.Object);
        }

        //[Fact]
        //public async Task Handle_ShouldRegisterNewUser_WhenUserDoesNotExist()
        //{
        //    // Arrange
        //    var googleIdToken = "valid_id_token";
        //    var googlePayload = new GoogleJsonWebSignature.Payload
        //    {
        //        Email = "newuser@example.com",
        //        GivenName = "New",
        //        FamilyName = "User",
        //        Picture = "http://example.com/avatar.jpg"
        //    };

        //    var newUser = new User
        //    {
        //        Email = googlePayload.Email,
        //        FirstName = googlePayload.GivenName,
        //        LastName = googlePayload.FamilyName,
        //        AvatarUrl = googlePayload.Picture
        //    };

        //    var newUserDto = new UserDto
        //    {
        //        Email = googlePayload.Email
        //    };

        //    // Mock Google token validation
        //    _googleAuthServiceMock.Setup(service => service.ValidateAsync(It.IsAny<string>()))
        //                          .ReturnsAsync(googlePayload);

        //    _userRepoMock.Setup(repo => repo.AddAsync(It.IsAny<User>()))
        //                 .ReturnsAsync(newUser); // Return newUser wrapped in a Task

        //    _tokenServiceMock.Setup(ts => ts.GenerateJwt(It.IsAny<User>()))
        //                     .Returns("fake_jwt_token");

        //    // Act
        //    var result = await _handler.Handle(new GoogleLoginCommand(googleIdToken), CancellationToken.None);

        //    // Assert
        //    Assert.NotNull(result);
        //    Assert.Equal("Registration successful, user logged in.", result.Message);
        //    Assert.NotNull(result.AccessToken);
        //    Assert.Equal("newuser@example.com", result.Data.User.Email);
        //    _userRepoMock.Verify(repo => repo.AddAsync(It.Is<User>(u => u.Email == googlePayload.Email)), Times.Once);
    }

    //[Fact]
    //public async Task Handle_ShouldLoginExistingUser_WhenUserExists()
    //{
    //    // Arrange
    //    var googleIdToken = "valid_id_token";
    //    var googlePayload = new GoogleJsonWebSignature.Payload
    //    {
    //        Email = "existinguser@example.com",
    //        GivenName = "Existing",
    //        FamilyName = "User",
    //    };

    //    var existingUser = new User
    //    {
    //        Email = googlePayload.Email,
    //        FirstName = googlePayload.GivenName,
    //        LastName = googlePayload.FamilyName,
    //    };

    //    var userDto = new UserDto
    //    {
    //        Email = googlePayload.Email
    //    };

    //    // Mock Google token validation
    //    _googleAuthServiceMock.Setup(service => service.ValidateAsync(It.IsAny<string>()))
    //                          .ReturnsAsync(googlePayload);

    //    var dbUserQueryable = new List<User>().AsQueryable();

    //    _userRepoMock.Setup(x => x.GetQueryableBySpec(It.IsAny<Expression<Func<User, bool>>>()))
    //        .Returns(dbUserQueryable);


    //    _tokenServiceMock.Setup(ts => ts.GenerateJwt(It.IsAny<User>()))
    //                     .Returns("fake_jwt_token");

    //    // Act
    //    var result = await _handler.Handle(new GoogleLoginCommand(googleIdToken), CancellationToken.None);

    //    // Assert
    //    Assert.NotNull(result);
    //    Assert.Equal("Login successful", result.Message);
    //    Assert.NotNull(result.AccessToken);
    //    Assert.Equal("existinguser@example.com", result.Data.User.Email);
    //    _userRepoMock.Verify(repo => repo.AddAsync(It.IsAny<User>()), Times.Never);
    //}
}

