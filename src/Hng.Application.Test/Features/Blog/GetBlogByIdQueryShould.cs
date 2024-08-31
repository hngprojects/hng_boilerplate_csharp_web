using System.Linq.Expressions;
using AutoMapper;
using FluentAssertions;
using Hng.Application.Features.Blogs.Dtos;
using Hng.Application.Features.Blogs.Handlers;
using Hng.Application.Features.Blogs.Queries;
using Hng.Application.Features.UserManagement.Dtos;
using Hng.Domain.Entities;
using Hng.Domain.Enums;
using Hng.Infrastructure.Repository.Interface;
using Microsoft.AspNetCore.Http;
using Moq;
using Xunit;

namespace Hng.Application.Test.Features.Blog
{
    public class GetBlogByIdQueryShould
    {
        private readonly Mock<IRepository<Domain.Entities.Blog>> _blogRepositoryMock;
        private readonly IMapper _mapper;
        private readonly GetBlogByIdQueryHandler _handler;

        public GetBlogByIdQueryShould()
        {
            _blogRepositoryMock = new Mock<IRepository<Domain.Entities.Blog>>();

            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Domain.Entities.Blog, BlogDto>()
                    .ForMember(dest => dest.AuthorId, opt => opt.MapFrom(src => src.Author.Id));
                cfg.CreateMap<User, UserDto>(); // Adjust if you have UserDto
            });

            _mapper = mapperConfig.CreateMapper();
            _handler = new GetBlogByIdQueryHandler(_mapper, _blogRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_BlogExists_ReturnsBlogDto()
        {
            // Arrange
            var blogId = Guid.NewGuid();
            var authorId = Guid.NewGuid();
            var blog = new Domain.Entities.Blog
            {
                Id = blogId,
                Title = "Test Blog",
                ImageUrl = "https://example.com/image.jpg",
                Content = "Blog content",
                PublishedDate = DateTime.UtcNow,
                Author = new User { Id = authorId, FirstName = "AuthorName" },
                Category = BlogCategory.Data
            };

            _blogRepositoryMock.Setup(repo => repo.GetBySpec(It.IsAny<Expression<Func<Domain.Entities.Blog, bool>>>()))
                .ReturnsAsync(blog);

            var query = new GetBlogByIdQuery(blogId);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Data.Should().NotBeNull();
            result.Data.Id.Should().Be(blogId);
            result.Data.Title.Should().Be(blog.Title);
            result.Data.ImageUrl.Should().Be(blog.ImageUrl);
            result.Data.Content.Should().Be(blog.Content);
            result.Data.PublishedDate.Should().Be(blog.PublishedDate);
            result.Data.AuthorId.Should().Be(authorId);
            result.Data.Category.Should().Be(blog.Category);
        }

        [Fact]
        public async Task Handle_BlogDoesNotExist_ReturnsNull()
        {
            // Arrange
            var blogId = Guid.NewGuid();

            _blogRepositoryMock.Setup(repo => repo.GetBySpec(It.IsAny<Expression<Func<Domain.Entities.Blog, bool>>>()))
                .ReturnsAsync((Domain.Entities.Blog)null);

            var query = new GetBlogByIdQuery(blogId);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Data.Should().BeNull();
            result.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        }
    }
}
