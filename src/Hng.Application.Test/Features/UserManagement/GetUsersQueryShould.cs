using System.Linq.Expressions;
using AutoMapper;
using Hng.Application.Features.UserManagement.Handlers;
using Hng.Application.Features.UserManagement.Mappers;
using Hng.Application.Features.UserManagement.Queries;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using Moq;
using Xunit;

namespace Hng.Application.Test.Features.UserManagement
{
    public class GetUsersQueryShould
    {
        private readonly IMapper _mapper;

        public GetUsersQueryShould()
        {
            var userMappingProfile = new UserMappingProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(userMappingProfile));
            _mapper = new Mapper(configuration);
        }

        [Fact]
        public async Task ReturnMultipleUsers()
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

            var userRepository = new Mock<IRepository<User>>(MockBehavior.Default);
            userRepository.Setup(s => s.GetAllAsync(It.IsAny<Expression<Func<User, object>>[]>()))
                .Returns(Task.FromResult(expectedList));

            var handler = new GetUsersQueryHandler(userRepository.Object, _mapper);

            // Act
            var results = await handler.Handle(new GetUsersQuery(), default);

            // Assert
            Assert.Equal(expectedList.Count(), results.Count());

            for (int index = 0; index < expectedList.Count(); index++)
            {
                var expected = expectedList.ElementAt(index);
                var result = results.ElementAt(index);

                Assert.Equal(expected.Id, result.Id);
                Assert.Equal(expected.Email, result.Email);
                Assert.Contains(expected.FirstName, result.Name);
                Assert.Contains(expected.LastName, result.Name);
            }
        }
    }
}