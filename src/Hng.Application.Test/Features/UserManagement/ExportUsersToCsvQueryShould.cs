using AutoMapper;
using Hng.Application.Features.UserManagement.Dtos;
using Hng.Application.Features.UserManagement.Handlers;
using Hng.Application.Features.UserManagement.Mappers;
using Hng.Application.Features.UserManagement.Queries;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Hng.Application.Test.Features.UserManagement
{
    public class ExportUsersToCsvQueryHandlerShould
    {
        private readonly IMapper _mapper;

        public ExportUsersToCsvQueryHandlerShould()
        {
<<<<<<< HEAD
            var userMappingProfile = new UserMappingProfile();  // Ensure your mapping profile is correctly configured
=======
            var userMappingProfile = new UserMappingProfile();  // Make sure your mapping profile is correctly configured
>>>>>>> ef132a9ca2db7d30f4ab936d4f3b1adb9edca513
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(userMappingProfile));
            _mapper = new Mapper(configuration);
        }

        [Fact]
        public async Task ExportUsersToCsvSuccessfully()
        {
            // Arrange
            var expectedList = new List<User>
            {
                new User
                {
                    Id = Guid.NewGuid(),
                    FirstName = "Jon",
                    LastName = "Snow",
                    Email = "jsnow@gmail.com"
                },
                new User
                {
                    Id = Guid.NewGuid(),
                    FirstName = "Arya",
                    LastName = "Stark",
                    Email = "arya@stark.com"
                }
            };

            var userRepositoryMock = new Mock<IRepository<User>>(MockBehavior.Default);
            userRepositoryMock.Setup(r => r.GetAllAsync())
                .ReturnsAsync(expectedList);

            var handler = new ExportUsersToCsvQueryHandler(userRepositoryMock.Object, _mapper);

            // Act
            var result = await handler.Handle(new ExportUsersToCsvQuery(), default);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Length > 0);  // Ensure that the result is not empty

            // Verify that the CSV is correctly formatted by reading it from the byte array
            using var memoryStream = new MemoryStream(result);
            using var streamReader = new StreamReader(memoryStream, Encoding.UTF8);
            var csvContent = await streamReader.ReadToEndAsync();

            // Check that the CSV contains the expected data
            foreach (var user in expectedList)
            {
                Assert.Contains(user.Id.ToString(), csvContent);
<<<<<<< HEAD
                Assert.Contains(user.FirstName, csvContent);
=======
                Assert.Contains(user.FirstName, csvContent); 
>>>>>>> ef132a9ca2db7d30f4ab936d4f3b1adb9edca513
                Assert.Contains(user.Email, csvContent);
            }
        }
    }
}