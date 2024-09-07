using AutoMapper;
using Hng.Application.Features.SuperAdmin.Dto;
using Hng.Application.Features.SuperAdmin.Handlers;
using Hng.Application.Features.SuperAdmin.Queries;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using Moq;
using Xunit;

namespace Hng.Application.Test.Features.SuperAdmin
{
    public class GetUsersBySearchQueryHandlerShould
    {
        private readonly Mock<IRepository<User>> _mockRepository;
        private readonly IMapper _mapper;

        public GetUsersBySearchQueryHandlerShould()
        {
            _mockRepository = new Mock<IRepository<User>>();

            // Set up AutoMapper with your profiles
            var config = new MapperConfiguration(cfg =>
            {
                // Add your AutoMapper profiles here
                cfg.CreateMap<User, UserSuperDto>();
            });

            _mapper = config.CreateMapper();
        }

        [Fact]
        public async Task ReturnMultipleUsersOnEmptyParameters()
        {
            // Arrange
            IEnumerable<User> expectedList = new List<User>{
                new() {
                    Id = Guid.NewGuid(),
                    Email = "js@gmail.com",
                    LastName = "Snow",
                    FirstName = "Jon"
                },
                new ()
                {
                    Id = Guid.NewGuid(),
                    Email = "kk@gmail.com",
                    LastName = "King",
                    FirstName = "Kong"
                }
            };

            var parameters = new UsersQueryParameters
            { };

            _mockRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(expectedList);
            var handler = new GetUsersBySearchQueryHandler(_mockRepository.Object, _mapper);

            // Act
            var results = await handler.Handle(new GetUsersBySearchQuery(parameters), CancellationToken.None);

            // Assert
            Assert.Equal(expectedList.Count(), results.Count());

            for (int index = 0; index < expectedList.Count(); index++)
            {
                var expected = expectedList.ElementAt(index);
                var result = results.ElementAt(index);

                Assert.Equal(expected.Id, result.Id);
                Assert.Equal(expected.Email, result.Email);
                Assert.Contains(expected.FirstName, result.FirstName);
                Assert.Contains(expected.LastName, result.LastName);
            }
        }

        [Fact]
        public async Task ReturnOneUserOnValidEmailParameters()
        {
            // Arrange
            var expectedCount = 1;
            var expectedemail = "js@gmail.com";
            IEnumerable<User> expectedList = new List<User>{
                new() {
                    Id = Guid.NewGuid(),
                    Email = "js@gmail.com",
                    LastName = "Snow",
                    FirstName = "Jon"
                },
                new ()
                {
                    Id = Guid.NewGuid(),
                    Email = "kk@gmail.com",
                    LastName = "King",
                    FirstName = "Kong"
                }
            };

            var parameters = new UsersQueryParameters
            {
                Email = expectedemail
            };

            _mockRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(expectedList);
            var handler = new GetUsersBySearchQueryHandler(_mockRepository.Object, _mapper);

            // Act
            var results = await handler.Handle(new GetUsersBySearchQuery(parameters), CancellationToken.None);

            // Assert
            Assert.Equal(expectedCount, results.Count());
            Assert.Equal(results.First().Email, expectedemail);
        }

        [Fact]
        public async Task ReturnUsersOnValidLastNameParameters()
        {
            // Arrange
            var expectedCount = 2;
            var expectedLastName = "Snow";
            IEnumerable<User> expectedList = new List<User>{
                new() {
                    Id = Guid.NewGuid(),
                    Email = "js@gmail.com",
                    LastName = "Snow",
                    FirstName = "Jon"
                },
                new ()
                {
                    Id = Guid.NewGuid(),
                    Email = "kk@gmail.com",
                    LastName = "King",
                    FirstName = "Kong"
                },
                new ()
                {
                    Id = Guid.NewGuid(),
                    Email = "jin@gmail.com",
                    LastName = "Snow",
                    FirstName = "Sarah"
                }
            };

            var parameters = new UsersQueryParameters
            {
                Lastname = expectedLastName
            };

            _mockRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(expectedList);
            var handler = new GetUsersBySearchQueryHandler(_mockRepository.Object, _mapper);

            // Act
            var results = await handler.Handle(new GetUsersBySearchQuery(parameters), CancellationToken.None);

            // Assert
            Assert.Equal(expectedCount, results.Count());
            Assert.Equal(results.First().LastName, expectedLastName);
            Assert.Equal(results.First().LastName, expectedLastName);
        }


    }
}
