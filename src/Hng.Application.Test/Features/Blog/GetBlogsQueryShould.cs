using AutoMapper;
using FluentAssertions;
using Hng.Application.Features.Blogs.Dtos;
using Hng.Application.Features.Blogs.Handlers;
using Hng.Application.Features.Blogs.Queries;
using Hng.Domain.Entities;
using Hng.Domain.Enums;
using Hng.Infrastructure.Repository.Interface;
using Moq;
using Xunit;

namespace Hng.Application.Test.Features.Blog
{
    public class GetBlogsQueryShould
    {
        private readonly Mock<IRepository<Domain.Entities.Blog>> _blogRepositoryMock;
        private readonly IMapper _mapper;
        private readonly GetBlogsQueryHandler _handler;

        public GetBlogsQueryShould()
        {
            _blogRepositoryMock = new Mock<IRepository<Domain.Entities.Blog>>();

            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Domain.Entities.Blog, BlogDto>()
                    .ForMember(dest => dest.Author, opt => opt.MapFrom(src => src.Author));
                cfg.CreateMap<User, User>();
            });

            _mapper = mapperConfig.CreateMapper();
            _handler = new GetBlogsQueryHandler(_blogRepositoryMock.Object, _mapper);
        }

        [Fact]
        public async Task Handle_BlogsExist_ReturnsListOfBlogDto()
        {
            // Arrange
            var blogs = new List<Domain.Entities.Blog>
            {
                new Domain.Entities.Blog
                {
                    Id = Guid.NewGuid(),
                    Title = "Test Blog 1",
                    ImageUrl = "https://example.com/image1.jpg",
                    Content = "Blog content 1",
                    PublishedDate = DateTime.UtcNow,
                    Author = new User { Id = Guid.NewGuid(), FirstName = "AuthorName1" },
                    Category = BlogCategory.Data
                },
                new Domain.Entities.Blog
                {
                    Id = Guid.NewGuid(),
                    Title = "Test Blog 2",
                    ImageUrl = "https://example.com/image2.jpg",
                    Content = "Blog content 2",
                    PublishedDate = DateTime.UtcNow,
                    Author = new User { Id = Guid.NewGuid(), FirstName = "AuthorName2" },
                    Category = BlogCategory.Programming
                }
            };

            _blogRepositoryMock.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(blogs);

            var query = new GetBlogsQuery();

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            var blogDtos = result as BlogDto[] ?? result.ToArray();
            blogDtos.Should().NotBeNull();
            blogDtos.Should().HaveCount(2);
            blogDtos.First().Title.Should().Be("Test Blog 1");
            blogDtos.Last().Title.Should().Be("Test Blog 2");
        }

        [Fact]
        public async Task Handle_NoBlogsExist_ReturnsEmptyList()
        {
            // Arrange
            _blogRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(new List<Domain.Entities.Blog>());

            var query = new GetBlogsQuery();

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            var blogDtos = result as BlogDto[] ?? result.ToArray();
            blogDtos.Should().NotBeNull();
            blogDtos.Should().BeEmpty();
        }
    }
}
