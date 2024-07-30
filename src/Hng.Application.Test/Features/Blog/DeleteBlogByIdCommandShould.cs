using System.Linq.Expressions;
using Hng.Application.Features.Blogs.Commands;
using Hng.Application.Features.Blogs.Handlers;
using Hng.Infrastructure.Repository.Interface;
using Moq;
using Xunit;

namespace Hng.Application.Test.Features.Blog
{
    public class DeleteBlogByIdCommandShould
    {
        private readonly Mock<IRepository<Domain.Entities.Blog>> _blogRepositoryMock;
        private readonly DeleteBlogByIdCommandHandler _handler;

        public DeleteBlogByIdCommandShould()
        {
            _blogRepositoryMock = new Mock<IRepository<Domain.Entities.Blog>>();
            _handler = new DeleteBlogByIdCommandHandler(_blogRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_BlogExists_DeletesBlog()
        {
            // Arrange
            var blogId = Guid.NewGuid();
            var blog = new Domain.Entities.Blog { Id = blogId };

            _blogRepositoryMock.Setup(repo => repo.GetBySpec(It.IsAny<Expression<Func<Domain.Entities.Blog, bool>>>()))
                .ReturnsAsync(blog);

            _blogRepositoryMock.Setup(repo => repo.SaveChanges())
                .Returns(Task.CompletedTask);

            var command = new DeleteBlogByIdCommand(blogId);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _blogRepositoryMock.Verify(repo => repo.GetBySpec(It.IsAny<Expression<Func<Domain.Entities.Blog, bool>>>()), Times.Once);
            _blogRepositoryMock.Verify(repo => repo.DeleteAsync(It.Is<Domain.Entities.Blog>(j => j.Id == blogId)), Times.Once);
            _blogRepositoryMock.Verify(repo => repo.SaveChanges(), Times.Once);;
        }

    }
}
