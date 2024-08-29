using AutoMapper;
using Hng.Application.Features.Categories.Commands;
using Hng.Application.Features.Categories.Dtos;
using Hng.Application.Features.Categories.Handlers.Commands;
using Hng.Application.Shared.Dtos;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using Moq;
using Xunit;
using Hng.Application.Features.Categories.Mappers;

namespace Hng.Application.Test.Features.Categories
{
    public class CreateCategoryCommandHandlerShould
    {
        private readonly Mock<IRepository<Category>> _categoryRepoMock;
        private readonly IMapper _mapper;

        public CreateCategoryCommandHandlerShould()
        {
            _categoryRepoMock = new Mock<IRepository<Category>>();

            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new CategoryMapperProfile());
            });
            _mapper = mapperConfig.CreateMapper();
        }

        [Fact]
        public async Task CreateCategory_ValidInput_ReturnsSuccessResponse()
        {
            // Arrange
            var handler = new CreateCategoryCommandHandler(_categoryRepoMock.Object, _mapper);
            var command = new CreateCategoryCommand("Test Category", "Test Description", "test-category");

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsType<SuccessResponseDto<CategoryDto>>(result);
            Assert.Equal(201, result.StatusCode);
            Assert.Equal("Test Category", result.Data.Name);
        }

        [Fact]
        public async Task CreateCategory_InvalidInput_ReturnsFailureResponse()
        {
            // Arrange
            var handler = new CreateCategoryCommandHandler(_categoryRepoMock.Object, _mapper);
            var command = new CreateCategoryCommand("", "", "");

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsType<SuccessResponseDto<CategoryDto>>(result);
            Assert.Equal(400, result.StatusCode);
            Assert.Null(result.Data);
            Assert.Equal("Category name is required.", result.Message);
        }
    }
}