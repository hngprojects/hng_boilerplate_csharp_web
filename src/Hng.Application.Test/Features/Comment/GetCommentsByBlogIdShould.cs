using System.Linq.Expressions;
using AutoMapper;
using Hng.Application.Features.Comments.Dtos;
using Hng.Application.Features.Comments.Handlers;
using Hng.Application.Features.Comments.Queries;
using Hng.Infrastructure.Repository.Interface;
using Moq;
using Xunit;

namespace Hng.Application.Test.Features.Comment
{
    public class GetCommentsByBlogIdQueryShould
    {
        private readonly Mock<IRepository<Domain.Entities.Comment>> _commentRepositoryMock;
        private readonly GetCommentsByBlogIdQueryHandler _handler;

        public GetCommentsByBlogIdQueryShould()
        {
            _commentRepositoryMock = new Mock<IRepository<Domain.Entities.Comment>>();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Domain.Entities.Comment, CommentDto>().ReverseMap();
            });

            var mapper = config.CreateMapper();
            _handler = new GetCommentsByBlogIdQueryHandler(mapper, _commentRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnComments_WhenBlogExists()
        {
            // Arrange
            var blogId = Guid.NewGuid();
            var comments = new List<Domain.Entities.Comment>
            {
                new() { Id = Guid.NewGuid(), BlogId = blogId, Content = "Test Comment 1" },
                new() { Id = Guid.NewGuid(), BlogId = blogId, Content = "Test Comment 2" }
            };
            var blog = new Domain.Entities.Blog { Id = blogId, Title = "Test Blog" };

            _commentRepositoryMock.Setup(r => r.GetAllBySpec(
                    It.IsAny<Expression<Func<Domain.Entities.Comment, bool>>>()))
                .ReturnsAsync(comments);

            var query = new GetCommentsByBlogIdQuery(blogId);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            var commentDtos = result as CommentDto[] ?? result.ToArray();
            Assert.Equal("Test Comment 1", commentDtos[0].Content);
            Assert.Equal("Test Comment 2", commentDtos[1].Content);
        }

    }
}
