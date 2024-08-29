using AutoMapper;
using Hng.Application.Features.Categories.Commands;
using Hng.Application.Features.Categories.Handlers.Commands;
using Hng.Application.Features.Categories.Mappers;
using Hng.Application.Shared.Dtos;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using Moq;
using Xunit;

namespace Hng.Application.Test.Features.Categories
{
    public class DeleteCategoryCommandHandlerShould
    {
        private readonly Mock<IRepository<Category>> _categoryRepoMock;
        private readonly IMapper _mapper;

        public DeleteCategoryCommandHandlerShould()
        {
            _categoryRepoMock = new Mock<IRepository<Category>>();

            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new CategoryMapperProfile());
            });
            _mapper = mapperConfig.CreateMapper();
        }

        [Fact]
        public async Task DeleteCategory_ExistingCategory_ReturnsSuccessResponse()
        {
            // Arrange
            var existingCategory = new Category { Id = Guid.NewGuid(), Name = "Existing Category" };
            _categoryRepoMock.Setup(repo => repo.GetAsync(It.IsAny<Guid>()))
                .ReturnsAsync(existingCategory);

            var handler = new DeleteCategoryCommandHandler(_categoryRepoMock.Object);
            var command = new DeleteCategoryCommand(existingCategory.Id);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsType<SuccessResponseDto<bool>>(result);
            Assert.Equal(200, result.StatusCode);
            Assert.True(result.Data);
        }

        [Fact]
        public async Task DeleteCategory_NonExistingCategory_ReturnsNotFoundResponse()
        {
            // Arrange
            _categoryRepoMock.Setup(repo => repo.GetAsync(It.IsAny<Guid>()))
                .ReturnsAsync((Category)null);

            var handler = new DeleteCategoryCommandHandler(_categoryRepoMock.Object);
            var command = new DeleteCategoryCommand(Guid.NewGuid());

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsType<SuccessResponseDto<bool>>(result);
            Assert.Equal(404, result.StatusCode);
            Assert.False(result.Data);
        }
    }
}