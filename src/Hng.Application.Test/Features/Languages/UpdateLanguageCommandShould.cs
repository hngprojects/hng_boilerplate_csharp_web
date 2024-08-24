using AutoMapper;
using Hng.Application.Features.Languages.Commands;
using Hng.Application.Features.Languages.Handlers.Connamds;
using Hng.Application.Features.Languages.Mappers;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Hng.Application.Test.Features.Languages
{
    public class UpdateLanguageCommandShould
    {
        private readonly IMapper _mapper;
        private readonly Mock<IRepository<Language>> _repositoryMock;
        private readonly UpdateLanguageCommandHandler _handler;

        public UpdateLanguageCommandShould()
        {
            var mappingProfile = new LanguageMapperProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(mappingProfile));
            _mapper = new Mapper(configuration);
            _repositoryMock = new Mock<IRepository<Language>>();
            _handler = new UpdateLanguageCommandHandler(_repositoryMock.Object, _mapper);
        }

        [Fact]
        public async Task Handle_ShouldReturnUpdatedLanguage()
        {
            // Arrange
            var expectedId = Guid.NewGuid();
            var updateDto = new UpdateLanguageCommand
            {
                Id = expectedId,
                Name = "Updated English"
            };
            var existingLanguage = new Language
            {
                Id = expectedId,
                Name = "English"
            };
            _repositoryMock.Setup(r => r.GetAsync(expectedId))
                .ReturnsAsync(existingLanguage);
            _repositoryMock.Setup(r => r.UpdateAsync(It.IsAny<Language>()))
                .Returns(Task.CompletedTask);
            _repositoryMock.Setup(r => r.SaveChanges())
                .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(updateDto, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal("Language updated successfully", result.Message);
            Assert.Equal(expectedId, result.Data.Id);
            Assert.Equal(updateDto.Name, result.Data.Name);
        }

        [Fact]
        public async Task Handle_ShouldReturnNotFound_WhenLanguageDoesNotExist()
        {
            // Arrange
            var nonExistentId = Guid.NewGuid();
            var updateDto = new UpdateLanguageCommand
            {
                Id = nonExistentId,
                Name = "Non-existent Language"
            };
            _repositoryMock.Setup(r => r.GetAsync(nonExistentId))
                .ReturnsAsync((Language)null);

            // Act
            var result = await _handler.Handle(updateDto, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(404, result.StatusCode);
            Assert.Equal("Language not found", result.Message);
            Assert.Null(result.Data);
        }
    }
}