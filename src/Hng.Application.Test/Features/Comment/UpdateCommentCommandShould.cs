using System.Linq.Expressions;
using AutoMapper;
using Hng.Application.Features.Comments.Commands;
using Hng.Application.Features.Comments.Dtos;
using Hng.Application.Features.Comments.Handlers;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using Hng.Infrastructure.Services.Interfaces;
using Moq;
using Xunit;

public class UpdateCommentCommandShould
{
    private readonly IMapper _mapper;
    private readonly Mock<IRepository<Comment>> _commentRepositoryMock;
    private readonly Mock<IAuthenticationService> _authenticationServiceMock;
    private readonly UpdateCommentCommandHandler _handler;

    public UpdateCommentCommandShould()
    {
        // Setup AutoMapper
        var configuration = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<UpdateCommentDto, Comment>().ReverseMap();
            cfg.CreateMap<Comment, CommentDto>().ReverseMap();
        });
        _mapper = configuration.CreateMapper();

        _commentRepositoryMock = new Mock<IRepository<Comment>>();
        _authenticationServiceMock = new Mock<IAuthenticationService>();
        _handler = new UpdateCommentCommandHandler(
            _mapper,
            _commentRepositoryMock.Object,
            _authenticationServiceMock.Object
        );
    }

    [Fact]
    public async Task Handle_ShouldUpdateCommentSuccessfully()
    {
        // Arrange
        var blogId = Guid.NewGuid();
        var commentId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var updateCommentDto = new UpdateCommentDto { Content = "Updated content" };
        var comment = new Comment
        {
            Id = commentId,
            BlogId = blogId,
            AuthorId = userId,
            Content = "Original content",
            UpdatedAt = DateTime.UtcNow
        };

        _commentRepositoryMock
            .Setup(r => r.GetBySpec(It.IsAny<Expression<Func<Comment, bool>>>()))
            .ReturnsAsync(comment);

        _authenticationServiceMock
            .Setup(s => s.GetCurrentUserAsync())
            .ReturnsAsync(userId);

        _commentRepositoryMock
            .Setup(r => r.UpdateAsync(It.IsAny<Comment>()))
            .Returns(Task.CompletedTask);

        _commentRepositoryMock
            .Setup(r => r.SaveChanges())
            .Returns(Task.CompletedTask);

        // Act
        var result = await _handler.Handle(
            new UpdateCommentCommand(blogId, commentId, updateCommentDto, userId),
            CancellationToken.None
        );

        // Assert
        Assert.NotNull(result);
        Assert.Equal(200, result.StatusCode);
        Assert.Equal("Comment updated successfully", result.Message);
        Assert.Equal("Updated content", result.Data.Content);
        _commentRepositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Comment>()), Times.Once);
        _commentRepositoryMock.Verify(r => r.SaveChanges(), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnNotFound_WhenCommentNotFound()
    {
        // Arrange
        var blogId = Guid.NewGuid();
        var commentId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var updateCommentDto = new UpdateCommentDto { Content = "Updated content" };

        _commentRepositoryMock
            .Setup(r => r.GetBySpec(It.IsAny<Expression<Func<Comment, bool>>>()))
            .ReturnsAsync((Comment)null);

        _authenticationServiceMock
            .Setup(s => s.GetCurrentUserAsync())
            .ReturnsAsync(userId);

        // Act
        var result = await _handler.Handle(
            new UpdateCommentCommand(blogId, commentId, updateCommentDto, userId),
            CancellationToken.None
        );

        // Assert
        Assert.NotNull(result);
        Assert.Equal(404, result.StatusCode);
        Assert.Equal("Comment not found.", result.Message);
        Assert.Null(result.Data);
    }

    [Fact]
    public async Task Handle_ShouldReturnUnauthorized_WhenUserIsNotAuthorized()
    {
        // Arrange
        var blogId = Guid.NewGuid();
        var commentId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var anotherUserId = Guid.NewGuid();
        var updateCommentDto = new UpdateCommentDto { Content = "Updated content" };
        var comment = new Comment
        {
            Id = commentId,
            BlogId = blogId,
            AuthorId = userId,
            Content = "Original content",
            UpdatedAt = DateTime.UtcNow
        };

        _commentRepositoryMock
            .Setup(r => r.GetBySpec(It.IsAny<Expression<Func<Comment, bool>>>()))
            .ReturnsAsync(comment);

        _authenticationServiceMock
            .Setup(s => s.GetCurrentUserAsync())
            .ReturnsAsync(anotherUserId);

        // Act
        var result = await _handler.Handle(
            new UpdateCommentCommand(blogId, commentId, updateCommentDto, anotherUserId),
            CancellationToken.None
        );

        // Assert
        Assert.NotNull(result);
        Assert.Equal(403, result.StatusCode);
        Assert.Equal("You are not authorized to update this comment.", result.Message);
        Assert.Null(result.Data);
    }

    [Fact]
    public async Task Handle_ShouldReturnBadRequest_WhenContentIsEmpty()
    {
        // Arrange
        var blogId = Guid.NewGuid();
        var commentId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var updateCommentDto = new UpdateCommentDto { Content = string.Empty };
        var comment = new Comment
        {
            Id = commentId,
            BlogId = blogId,
            AuthorId = userId,
            Content = "Original content",
            UpdatedAt = DateTime.UtcNow
        };

        _commentRepositoryMock
            .Setup(r => r.GetBySpec(It.IsAny<Expression<Func<Comment, bool>>>()))
            .ReturnsAsync(comment);

        _authenticationServiceMock
            .Setup(s => s.GetCurrentUserAsync())
            .ReturnsAsync(userId);

        // Act
        var result = await _handler.Handle(
            new UpdateCommentCommand(blogId, commentId, updateCommentDto, userId),
            CancellationToken.None
        );

        // Assert
        Assert.NotNull(result);
        Assert.Equal(400, result.StatusCode);
        Assert.Equal("Comment cannot be empty.", result.Message);
        Assert.Null(result.Data);
    }
}