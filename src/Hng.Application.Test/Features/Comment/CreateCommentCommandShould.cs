using System.Linq.Expressions;
using AutoMapper;
using Hng.Application.Features.Comments.Commands;
using Hng.Application.Features.Comments.Dtos;
using Hng.Application.Features.Comments.Handlers;
using Hng.Infrastructure.Repository.Interface;
using Hng.Infrastructure.Services.Interfaces;
using Moq;
using Xunit;

namespace Hng.Application.Test.Features.Comment
{
    public class CreateCommentCommandShould
    {
        private readonly IMapper _mapper;
        private readonly Mock<IRepository<Domain.Entities.Comment>> _commentRepositoryMock;
        private readonly Mock<IRepository<Domain.Entities.Blog>> _blogRepositoryMock;
        private readonly Mock<IAuthenticationService> _authenticationServiceMock;
        private readonly CreateCommentCommandHandler _handler;

        public CreateCommentCommandShould()
        {
            // Setup AutoMapper
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CreateCommentDto, Domain.Entities.Comment>()
                    .ReverseMap();
                cfg.CreateMap<Domain.Entities.Comment, CommentDto>()
                    .ReverseMap();
            });
            _mapper = configuration.CreateMapper();

            _commentRepositoryMock = new Mock<IRepository<Domain.Entities.Comment>>();
            _blogRepositoryMock = new Mock<IRepository<Domain.Entities.Blog>>();
            _authenticationServiceMock = new Mock<IAuthenticationService>();
            _handler = new CreateCommentCommandHandler(
                _mapper,
                _commentRepositoryMock.Object,
                _blogRepositoryMock.Object,
                _authenticationServiceMock.Object
            );
        }

        [Fact]
        public async Task Handle_ShouldCreateCommentSuccessfully()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var blogId = Guid.NewGuid();
            var commentDto = new CreateCommentDto { Content = "Great post!" };
            var comment = new Domain.Entities.Comment { Content = "Great post!" };
            var blog = new Domain.Entities.Blog { Id = blogId, Comments = new List<Domain.Entities.Comment>() };

            // Configure mocks
            _authenticationServiceMock.Setup(s => s.GetCurrentUserAsync()).ReturnsAsync(userId);
            _blogRepositoryMock.Setup(r => r.GetBySpec(It.IsAny<Expression<Func<Domain.Entities.Blog, bool>>>())).ReturnsAsync(blog);

            // Create Mapper Configuration
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CreateCommentDto, Domain.Entities.Comment>();
                cfg.CreateMap<Domain.Entities.Comment, CommentDto>();
            });
            var mapper = configuration.CreateMapper();

            // Create handler with the new mapper instance
            var handler = new CreateCommentCommandHandler(
                mapper,
                _commentRepositoryMock.Object,
                _blogRepositoryMock.Object,
                _authenticationServiceMock.Object
            );

            // Setup the comment repository mock to return a task with the comment entity
            _commentRepositoryMock.Setup(r => r.AddAsync(It.IsAny<Domain.Entities.Comment>()))
                .Callback<Domain.Entities.Comment>(c =>
                {
                    c.AuthorId = userId;
                    c.BlogId = blogId;
                    blog.Comments.Add(c);
                })
                .ReturnsAsync(comment);

            _commentRepositoryMock.Setup(r => r.SaveChanges()).Returns(Task.CompletedTask);
            _blogRepositoryMock.Setup(r => r.SaveChanges()).Returns(Task.CompletedTask);

            // Act
            var result = await handler.Handle(new CreateCommentCommand(commentDto, blogId), CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Great post!", result.Content);
            Assert.Contains(blog.Comments, c => c.Content == "Great post!" && c.AuthorId == userId && c.BlogId == blogId);
            _commentRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Domain.Entities.Comment>()), Times.Once);
            _commentRepositoryMock.Verify(r => r.SaveChanges(), Times.Once);
            _blogRepositoryMock.Verify(r => r.SaveChanges(), Times.Once);
        }




        [Fact]
        public async Task Handle_ShouldThrowExceptionWhenBlogNotFound()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var blogId = Guid.NewGuid();
            var commentDto = new CreateCommentDto { Content = "Great post!" };

            _authenticationServiceMock.Setup(s => s.GetCurrentUserAsync()).ReturnsAsync(userId);
            _blogRepositoryMock.Setup(r => r.GetBySpec(It.IsAny<Expression<Func<Domain.Entities.Blog, bool>>>())).ReturnsAsync((Domain.Entities.Blog)null);

            // Act & Assert
            await Assert.ThrowsAsync<ApplicationException>(() =>
                _handler.Handle(new CreateCommentCommand(commentDto, blogId), CancellationToken.None)
            );
        }


        [Fact]
        public async Task Handle_ShouldThrowExceptionWhenUserIdIsNotAvailable()
        {
            // Arrange
            var blogId = Guid.NewGuid();
            var commentDto = new CreateCommentDto { Content = "Great post!" };

            _authenticationServiceMock.Setup(s => s.GetCurrentUserAsync()).ReturnsAsync(Guid.Empty);
            _blogRepositoryMock.Setup(r => r.GetBySpec(It.IsAny<Expression<Func<Domain.Entities.Blog, bool>>>())).ReturnsAsync(new Domain.Entities.Blog { Id = blogId });

            // Act & Assert
            await Assert.ThrowsAsync<ApplicationException>(() =>
                _handler.Handle(new CreateCommentCommand(commentDto, blogId), CancellationToken.None)
            );
        }
    }
}
