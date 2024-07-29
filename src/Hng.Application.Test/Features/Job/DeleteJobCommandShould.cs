using System.Linq.Expressions;
using Hng.Application.Features.Jobs.Commands;
using Hng.Application.Features.Jobs.Handlers;
using Hng.Infrastructure.Repository.Interface;
using Moq;
using Xunit;

namespace Hng.Application.Test.Features.Job
{
    public class DeleteJobCommandShould
    {
        private readonly Mock<IRepository<Domain.Entities.Job>> _mockJobRepository;
        private readonly DeleteJobCommandHandler _handler;

        public DeleteJobCommandShould()
        {
            _mockJobRepository = new Mock<IRepository<Domain.Entities.Job>>();
            _handler = new DeleteJobCommandHandler(_mockJobRepository.Object);
        }

        [Fact]
        public async Task Handle_ExistingJob_ShouldDelete()
        {
            // Arrange
            var jobId = Guid.NewGuid();
            var job = new Domain.Entities.Job { Id = jobId, Title = "Test Job" };

            _mockJobRepository.Setup(repo => repo.GetBySpec(It.IsAny<Expression<Func<Domain.Entities.Job, bool>>>()))
                .ReturnsAsync(job);

            // Adjust DeleteAsync setup to match Task<T> return type
            _mockJobRepository.Setup(repo => repo.DeleteAsync(It.IsAny<Domain.Entities.Job>()))
                .ReturnsAsync(job);

            _mockJobRepository.Setup(repo => repo.SaveChanges())
                .Returns(Task.CompletedTask);

            var command = new DeleteJobCommand(jobId);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mockJobRepository.Verify(repo => repo.GetBySpec(It.IsAny<Expression<Func<Domain.Entities.Job, bool>>>()), Times.Once);
            _mockJobRepository.Verify(repo => repo.DeleteAsync(It.Is<Domain.Entities.Job>(j => j.Id == jobId)), Times.Once);
            _mockJobRepository.Verify(repo => repo.SaveChanges(), Times.Once);
        }

        [Fact]
        public async Task Handle_NonExistingJob_ShouldNotDelete()
        {
            // Arrange
            var jobId = Guid.NewGuid();

            _mockJobRepository.Setup(repo => repo.GetBySpec(It.IsAny<Expression<Func<Domain.Entities.Job, bool>>>()))
                .ReturnsAsync((Domain.Entities.Job)null);

            _mockJobRepository.Setup(repo => repo.DeleteAsync(It.IsAny<Domain.Entities.Job>()))
                .ReturnsAsync((Domain.Entities.Job)null);

            _mockJobRepository.Setup(repo => repo.SaveChanges())
                .Returns(Task.CompletedTask);

            var command = new DeleteJobCommand(jobId);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mockJobRepository.Verify(repo => repo.GetBySpec(It.IsAny<Expression<Func<Domain.Entities.Job, bool>>>()), Times.Once);
            _mockJobRepository.Verify(repo => repo.DeleteAsync(It.IsAny<Domain.Entities.Job>()), Times.Never);
            _mockJobRepository.Verify(repo => repo.SaveChanges(), Times.Never);
        }

        [Fact]
        public async Task Handle_ExceptionThrown_ShouldPropagateException()
        {
            // Arrange
            var jobId = Guid.NewGuid();
            var job = new Domain.Entities.Job { Id = jobId, Title = "Test Job" };

            _mockJobRepository.Setup(repo => repo.GetBySpec(It.IsAny<Expression<Func<Domain.Entities.Job, bool>>>()))
                .ReturnsAsync(job);

            _mockJobRepository.Setup(repo => repo.DeleteAsync(It.IsAny<Domain.Entities.Job>()))
                .ThrowsAsync(new Exception("Test exception"));

            _mockJobRepository.Setup(repo => repo.SaveChanges())
                .Returns(Task.CompletedTask);

            var command = new DeleteJobCommand(jobId);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _handler.Handle(command, CancellationToken.None));
        }
    }
}
