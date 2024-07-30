using Hng.Application.Features.UserManagement.Dtos;
using Hng.Application.Features.UserManagement.Handlers;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using Hng.Infrastructure.Services.Interfaces;
using Moq;
using System.Linq.Expressions;
using Xunit;

namespace Hng.Application.Test.Features.UserManagement
{
    public class UpdatePasswordShould
    {
        private readonly Mock<IRepository<User>> _userRepositoryMock;
        private readonly Mock<IPasswordService> _passwordServiceMock;
        private readonly Mock<ITokenService> _tokenServiceMock;
        private readonly ChangePasswordHandler _handler;

        public UpdatePasswordShould()
        {
            _userRepositoryMock = new Mock<IRepository<User>>();
            _passwordServiceMock = new Mock<IPasswordService>();
            _tokenServiceMock = new Mock<ITokenService>();
            _handler = new ChangePasswordHandler(
                _userRepositoryMock.Object,
                _passwordServiceMock.Object,
                _tokenServiceMock.Object);
        }

        [Fact]
        public async Task UpdatePasswordReturnTrue()
        {
            // Arrange
            var user = new User() { Email = "ken@yopmail", Password = "^tghdt%$re#$$3", PasswordSalt = "$$%$%ffddff-78yh--hhgg" };
            var request = new ChangePasswordCommand()
            { OldPassword = "^tghdt%$re#$$3", NewPassword = "ouPwKrNOF]'7Srp", ConfirmNewPassword = "ouPwKrNOF]'7Srp" };

            _tokenServiceMock.Setup(x => x.GetCurrentUserEmail()).Returns(user.Email);

            _userRepositoryMock.Setup(repo => repo.GetBySpec(It.IsAny<Expression<Func<User, bool>>>()))
                .ReturnsAsync(user);

            _passwordServiceMock.Setup(service => service.IsPasswordEqual(request.OldPassword, user.PasswordSalt, user.Password))
                .Returns(true);

            _passwordServiceMock.Setup(x => x.GeneratePasswordSaltAndHash(It.IsAny<string>())).Returns((default, default));

            // Act
            var result = _handler.Handle(request, CancellationToken.None);
            
            // Assert
            Assert.NotNull(result);
            Assert.True(result.Result.IsSuccess);
        }
    }
}