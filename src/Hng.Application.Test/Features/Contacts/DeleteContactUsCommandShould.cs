using Hng.Application.Features.ContactsUs.Command;
using Hng.Application.Features.ContactsUs.Handlers;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Hng.Application.Test.Features.Contacts
{
    public class DeleteContactUsCommandHandlerTests
    {
        private readonly Mock<IRepository<ContactUs>> _repositoryMock;
        private readonly DeleteContactUsCommandHandler _handler;

        public DeleteContactUsCommandHandlerTests()
        {
            _repositoryMock = new Mock<IRepository<ContactUs>>();
            _handler = new DeleteContactUsCommandHandler(_repositoryMock.Object);
        }

        [Fact]
        public async Task Handle_Should_Delete_ContactUs_Successfully()
        {
            // Arrange
            var id = Guid.NewGuid();
            var contactUs = new ContactUs { Id = id };

            _repositoryMock.Setup(r => r.GetAsync(id)).ReturnsAsync(contactUs);
            _repositoryMock.Setup(r => r.DeleteAsync(contactUs)).Returns(Task.FromResult(contactUs));

            var command = new DeleteContactUsCommand(id);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(200, result.StatusCode);
            Assert.Equal("ContactUs deleted successfully", result.Message);
            _repositoryMock.Verify(r => r.GetAsync(id), Times.Once);
            _repositoryMock.Verify(r => r.DeleteAsync(contactUs), Times.Once);
            _repositoryMock.Verify(r => r.SaveChanges(), Times.Once);
        }

        [Fact]
        public async Task Handle_Should_Return_NotFound_When_ContactUs_Not_Found()
        {
            // Arrange
            var id = Guid.NewGuid();
            _repositoryMock.Setup(r => r.GetAsync(id)).ReturnsAsync((ContactUs)null);

            var command = new DeleteContactUsCommand(id);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(404, result.StatusCode);
            Assert.Equal("ContactUs not found", result.Message);
            _repositoryMock.Verify(r => r.GetAsync(id), Times.Once);
            _repositoryMock.Verify(r => r.DeleteAsync(It.IsAny<ContactUs>()), Times.Never);
        }
    }

}
