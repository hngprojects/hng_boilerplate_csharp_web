using System.Linq.Expressions;
using AutoMapper;
using Hng.Application.Features.Blogs.Commands;
using Hng.Application.Features.Blogs.Dtos;
using Hng.Application.Features.Blogs.Handlers;
using Hng.Domain.Enums;
using Hng.Infrastructure.Repository.Interface;
using Hng.Infrastructure.Services.Interfaces;
using Moq;
using Xunit;

namespace Hng.Application.Test.Features.Blog
{
    public class UpdateBlogCommandShould
    {
        private readonly Mock<IRepository<Domain.Entities.Blog>> _blogRepositoryMock;
        private readonly Mock<IAuthenticationService> _authenticationServiceMock;
        private readonly UpdateBlogCommandHandler _handler;

        public UpdateBlogCommandShould()
        {
            // Configure AutoMapper with real profiles
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<UpdateBlogDto, Domain.Entities.Blog>()
                    .ForMember(dest => dest.Id, opt => opt.Ignore())
                    .ForMember(dest => dest.AuthorId, opt => opt.Ignore())
                    .ForMember(dest => dest.Comments, opt => opt.Ignore())
                    .ForMember(dest => dest.UpdatedDate, opt => opt.Ignore());
                cfg.CreateMap<Domain.Entities.Blog, BlogDto>();
            });

            var mapper = mapperConfig.CreateMapper();

            _blogRepositoryMock = new Mock<IRepository<Domain.Entities.Blog>>();
            _authenticationServiceMock = new Mock<IAuthenticationService>();
            _handler = new UpdateBlogCommandHandler(mapper, _blogRepositoryMock.Object, _authenticationServiceMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldUpdateBlog_WhenUserIsAuthorized()
        {
            // Arrange
            var blogId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var updateBlogDto = new UpdateBlogDto
            {
                Title = "Updated Title",
                Content = "Updated Content",
                Category = BlogCategory.Software,
                ImageUrl = "updated_image_url"
            };

            var existingBlog = new Domain.Entities.Blog
            {
                Id = blogId,
                AuthorId = userId,
                Title = "Old Title",
                Content = "Old Content",
                Category = BlogCategory.Software,
                ImageUrl = "old_image_url"
            };

            _blogRepositoryMock.Setup(repo => repo.GetBySpec(It.IsAny<Expression<Func<Domain.Entities.Blog, bool>>>()))
                .ReturnsAsync(existingBlog);

            _authenticationServiceMock.Setup(auth => auth.GetCurrentUserAsync())
                .ReturnsAsync(userId);

            // Act
            var command = new UpdateBlogCommand(updateBlogDto, blogId);
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(updateBlogDto.Title, existingBlog.Title);
            Assert.Equal(updateBlogDto.Content, existingBlog.Content);
            Assert.Equal(updateBlogDto.Category, existingBlog.Category);
            Assert.Equal(updateBlogDto.ImageUrl, existingBlog.ImageUrl);
            _blogRepositoryMock.Verify(repo => repo.UpdateAsync(existingBlog), Times.Once);
            _blogRepositoryMock.Verify(repo => repo.SaveChanges(), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldThrowUnauthorizedAccessException_WhenUserIsNotAuthorized()
        {
            // Arrange
            var blogId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var unauthorizedUserId = Guid.NewGuid();

            var existingBlog = new Domain.Entities.Blog
            {
                Id = blogId,
                AuthorId = userId
            };

            _blogRepositoryMock.Setup(repo => repo.GetBySpec(It.IsAny<Expression<Func<Domain.Entities.Blog, bool>>>()))
                .ReturnsAsync(existingBlog);

            _authenticationServiceMock.Setup(auth => auth.GetCurrentUserAsync())
                .ReturnsAsync(unauthorizedUserId);

            // Act & Assert
            var command = new UpdateBlogCommand(new UpdateBlogDto(), blogId);
            await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _handler.Handle(command, CancellationToken.None));
        }
        
    }
}
