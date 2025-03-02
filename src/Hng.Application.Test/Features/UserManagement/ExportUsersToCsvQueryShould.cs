using System.Net.Mime;
using Hng.Application.Features.UserManagement.Queries;
using Hng.Application.Shared.Dtos;
using Hng.Web.Controllers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Hng.Application.Test.Features.UserManagement
{
    public class ExportUsersToCsvEndpointTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly UserController _controller;

        public ExportUsersToCsvEndpointTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _controller = new UserController(_mediatorMock.Object);
        }

        [Fact]
        public async Task ExportUsersToCsv_ReturnsCsvFile()
        {
            // Arrange
            var csvBytes = new byte[] { 1, 2, 3 }; // Simulated CSV file content
            _mediatorMock
                .Setup(m => m.Send(It.IsAny<ExportUsersToCsvQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(csvBytes);

            // Act
            var result = await _controller.ExportUsersToCsv();

            // Assert
            var fileResult = Assert.IsType<FileContentResult>(result);
            Assert.Equal(MediaTypeNames.Application.Octet, fileResult.ContentType);

            // Check if the filename starts with "users_export_" and ends with ".csv"
            Assert.StartsWith("users_export_", fileResult.FileDownloadName);
            Assert.EndsWith(".csv", fileResult.FileDownloadName);

            Assert.Equal(csvBytes, fileResult.FileContents);
        }

        [Fact]
        public async Task ExportUsersToCsv_UnauthorizedAccess_Returns401Unauthorized()
        {
            // Arrange
            _mediatorMock
                .Setup(m => m.Send(It.IsAny<ExportUsersToCsvQuery>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new UnauthorizedAccessException());

            // Act
            var result = await _controller.ExportUsersToCsv();

            // Assert
            var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);
            var response = Assert.IsType<FailureResponseDto<object>>(unauthorizedResult.Value);
            Assert.Equal(401, response.StatusCode);
            Assert.Equal("Unauthorized access. You must have Super Admin privileges to export user data.", response.Message);
        }

   
    }
}