using AutoMapper;
using Hng.Application.Features.ContactsUs.Dtos;
using Hng.Application.Features.ContactsUs.Handlers;
using Hng.Application.Features.ContactsUs.Queries;
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
    public class GetAllContactUsQueryShould
    {
        private readonly Mock<IRepository<ContactUs>> _repositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly GetAllContactUsQueryHandler _handler;

        public GetAllContactUsQueryShould()
        {
            _repositoryMock = new Mock<IRepository<ContactUs>>();
            _mapperMock = new Mock<IMapper>();
            _handler = new GetAllContactUsQueryHandler(_repositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_Should_Return_ContactUs_List_Successfully()
        {
            // Arrange
            var contactMessages = new List<ContactUs>
            {
                new ContactUs { Id = Guid.NewGuid(), FullName = "John Doe", Email = "john@example.com", PhoneNumber = "07051469638", Message = "Test Message" }
            };

            var contactUsResponseDtos = new List<ContactUsResponseDto>
            {
                new ContactUsResponseDto { Id = Guid.NewGuid(), FullName = "John Doe", Email = "john@example.com", PhoneNumber = "07051469638", Message = "Test Message" }
            };

            _repositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(contactMessages);
            _mapperMock.Setup(m => m.Map<List<ContactUsResponseDto>>(contactMessages)).Returns(contactUsResponseDtos);

            var query = new GetAllContactUsQuery();

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal("Request completed successfully", result.Message);
            Assert.NotEmpty(result.Data);
            _repositoryMock.Verify(r => r.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task Handle_Should_Return_Empty_List_When_No_ContactUs_Found()
        {
            // Arrange
            var contactMessages = new List<ContactUs>
            {
                new ContactUs { Id = Guid.NewGuid(), FullName = "John Doe", Email = "john@example.com", PhoneNumber = "07051469638", Message = "Test Message" }
            };

            var contactUsResponseDtos = new List<ContactUsResponseDto>
            {
                new ContactUsResponseDto { Id = Guid.NewGuid(), FullName = "John Doe", Email = "john@example.com", PhoneNumber = "07051469638", Message = "Test Message" }
            };

            // Mock the repository to return null to simulate the scenario of no data found
            _repositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync((List<ContactUs>)null);

            var query = new GetAllContactUsQuery();

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
            Assert.Equal("Request  Failed", result.Message);
        }

    }

}
