using AutoMapper;
using Hng.Application.Features.ContactsUs.Command;
using Hng.Application.Features.ContactsUs.Dtos;
using Hng.Application.Features.ContactsUs.Handlers;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Hng.Application.Test.Features.Contacts
{
    public class CreateContactUsCommandShould
    {
        private readonly Mock<IRepository<ContactUs>> _repositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly CreateContactUsCommandHandler _handler;

        public CreateContactUsCommandShould()
        {
            _repositoryMock = new Mock<IRepository<ContactUs>>();
            _mapperMock = new Mock<IMapper>();
            _handler = new CreateContactUsCommandHandler(_repositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_Should_Create_ContactUs_Successfully()
        {
            // Arrange
            var contactUsRequestDto = new ContactUsRequestDto
            {
                FullName = "John Doe",
                Email = "john@example.com",
                PhoneNumber = "08087654345",
                Message = "Test Message"
            };

            var contactUsEntity = new ContactUs
            {
                Id = Guid.NewGuid()
            };
            var contactUsResponseDto = new ContactUsResponseDto();

            _mapperMock.Setup(m => m.Map<ContactUs>(contactUsRequestDto)).Returns(contactUsEntity);
            _repositoryMock.Setup(r => r.AddAsync(contactUsEntity)).Returns(Task.FromResult(contactUsEntity));
            _repositoryMock.Setup(r => r.SaveChanges()).Returns(Task.CompletedTask);
            _mapperMock.Setup(m => m.Map<ContactUsResponseDto>(contactUsEntity)).Returns(contactUsResponseDto);

            var command = new CreateContactUsCommand(contactUsRequestDto);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(201, result.StatusCode);
            Assert.Equal("ContactUs created successfully", result.Message);
            Assert.Equal(contactUsResponseDto, result.Data);
            _repositoryMock.Verify(r => r.AddAsync(contactUsEntity), Times.Once);
            _repositoryMock.Verify(r => r.SaveChanges(), Times.Once);
        }

        [Fact]
        public async Task Handle_Should_Return_Error_When_Creation_Fails()
        {
            // Arrange
            var contactUsRequestDto = new ContactUsRequestDto
            {
                FullName = "John Doe",
                Email = "john@example.com",
                PhoneNumber = "08087654345",
                Message = "Test Message"
            };

            _mapperMock.Setup(m => m.Map<ContactUs>(contactUsRequestDto)).Returns((ContactUs)null);

            var command = new CreateContactUsCommand(contactUsRequestDto);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Null(result.Data);
            _repositoryMock.Verify(r => r.AddAsync(It.IsAny<ContactUs>()), Times.Never);
            _repositoryMock.Verify(r => r.SaveChanges(), Times.Never);
        }

    }
}
