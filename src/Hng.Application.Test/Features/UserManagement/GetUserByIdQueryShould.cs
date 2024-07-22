using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
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
    public class GetUserByIdShould
    {
        private readonly IMapper _mapper;

        public GetUserByIdShould()
        {
            var userMappingProfile = new UserMappingProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(userMappingProfile));
            _mapper = new Mapper(configuration);
        }

        [Fact]
        public async Task ReturnASingleUser()
        {
            // Arrange
            var expected = new User
            {
                Id = Guid.NewGuid(),
                Email = "js@gmail.com",
                LastName = "Snow",
                FirstName = "Jon"
            };

            var userRepository = new Mock<IRepository<User>>(MockBehavior.Default);
            userRepository.Setup(s => s.GetBySpec(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<Expression<Func<User, object>>[]>()))
                .Returns(Task.FromResult(expected));

            var handler = new GetUserByIdQueryHandler(userRepository.Object, _mapper);

            // Act
            var result = await handler.Handle(new GetUserByIdQuery(expected.Id), default);

            // Assert
            Assert.Equal(expected.Id, result.Id);
            Assert.Equal(expected.Email, result.Email);
            Assert.Contains(expected.FirstName, result.Name);
            Assert.Contains(expected.LastName, result.Name);
        }
    }
}