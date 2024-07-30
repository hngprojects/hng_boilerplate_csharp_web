using AutoMapper;
using Hng.Application.Features.Blogs.Commands;
using Hng.Application.Features.Blogs.Dtos;
using Hng.Application.Features.Blogs.Handlers;
using Hng.Application.Features.Blogs.Mappers;
using Hng.Domain.Enums;
using Hng.Infrastructure.Repository.Interface;
using Moq;
using Xunit;

namespace Hng.Application.Test.Features.Blog
{
    public class CreateBlogCommandShould
    {
        private readonly Mock<IRepository<Domain.Entities.Blog>> _mockBlogRepository;
        private readonly IMapper _mapper;
        private readonly CreateBlogCommandHandler _handler;

        public CreateBlogCommandShould()
        {
            _mockBlogRepository = new Mock<IRepository<Domain.Entities.Blog>>();

            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new BlogMapperProfile());
            });
            _mapper = configuration.CreateMapper();

            _handler = new CreateBlogCommandHandler(_mapper, _mockBlogRepository.Object);
        }

        [Fact]
        public async Task Handle_ValidRequest_ShouldCreateBlog()
        {
            // Arrange
            var createBlogDto = new CreateBlogDto
            {
                Title = "Test Title",
                Content = "Test Content",
                Category = BlogCategory.Data
            };

            var createBlogCommand = new CreateBlogCommand(createBlogDto);

            // Act
            var result = await _handler.Handle(createBlogCommand, CancellationToken.None);

            // Assert
            _mockBlogRepository.Verify(repo => repo.AddAsync(It.IsAny<Domain.Entities.Blog>()), Times.Once);
            _mockBlogRepository.Verify(repo => repo.SaveChanges(), Times.Once);
            
            Assert.NotNull(result);
            Assert.Equal("Test Title", result.Title);
            Assert.Equal("Test Content", result.Content);
            Assert.Equal(BlogCategory.Data, result.Category);
        }
    }
}