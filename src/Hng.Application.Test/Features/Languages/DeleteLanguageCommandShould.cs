using Hng.Application.Features.Languages.Commands;
using Hng.Application.Features.Languages.Handlers.Connamds;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using Moq;
using Xunit;

namespace Hng.Application.Test.Features.Languages
{
    public class DeleteLanguageCommandShould
    {
        private readonly Mock<IRepository<Language>> _repositoryMock;
        private readonly DeleteLanguageCommandHandler _handler;

        public DeleteLanguageCommandShould()
        {
            _repositoryMock = new Mock<IRepository<Language>>();
            _handler = new DeleteLanguageCommandHandler(_repositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnSuccessResponse_WhenLanguageExists()
        {
            // Arrange
            var expectedId = Guid.NewGuid();
            var deleteDto = new DeleteLanguageCommand
            {
                Id = expectedId
            };
            var existingLanguage = new Language
            {
                Id = expectedId,
                Name = "English"
            };
            _repositoryMock.Setup(r => r.GetAsync(expectedId))
                .ReturnsAsync(existingLanguage);

            _repositoryMock.Setup(r => r.DeleteAsync(It.IsAny<Language>()))
                .ReturnsAsync(existingLanguage);

            _repositoryMock.Setup(r => r.SaveChanges())
                .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(deleteDto, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal("Language deleted successfully", result.Message);
        }

        [Fact]
        public async Task Handle_ShouldReturnNotFound_WhenLanguageDoesNotExist()
        {
            // Arrange
            var nonExistentId = Guid.NewGuid();
            var deleteDto = new DeleteLanguageCommand
            {
                Id = nonExistentId
            };
            _repositoryMock.Setup(r => r.GetAsync(nonExistentId))
                .ReturnsAsync((Language)null);

            // Act
            var result = await _handler.Handle(deleteDto, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(404, result.StatusCode);
            Assert.Equal("Language not found", result.Message);
        }
    }
}