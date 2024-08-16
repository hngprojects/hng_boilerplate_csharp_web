using AutoMapper;
using Hng.Application.Features.Organisations.Mappers;
using Hng.Application.Features.Profiles.Mappers;
using Hng.Application.Features.UserManagement.Handlers;
using Hng.Application.Features.UserManagement.Mappers;
using Hng.Application.Features.UserManagement.Queries;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using Hng.Infrastructure.Services.Interfaces;
using Moq;
using System.Linq.Expressions;
using Xunit;

namespace Hng.Application.Test.Features.UserManagement
{
    public class GetCurrentlyLoggedInUserDetailsShould
    {
        private readonly Mock<IRepository<User>> _userRepository;
        private readonly Mock<IAuthenticationService> _authService;
        private readonly IMapper _mapper;

        public GetCurrentlyLoggedInUserDetailsShould()
        {
            var cfg = new MapperConfiguration(opt =>
            {
                opt.AddProfile<UserMappingProfile>();
                opt.AddProfile<ProfileMapperProfile>();
                opt.AddProfile<OrganizationMapperProfile>();
            });
            _mapper = cfg.CreateMapper();
            _userRepository = new Mock<IRepository<User>>();
            _authService = new Mock<IAuthenticationService>();
        }

        [Fact]
        public async Task Handler_Should_Return_LoggedIn_User_Details()
        {
            var userId = Guid.NewGuid();
            var sampleUser = new User
            {
                Id = userId,
                Email = "js@gmail.com",
                LastName = "Snow",
                FirstName = "Jon",
                Profile = new() { LastName = "Snow", FirstName = "Jon", Username = "Jon Snow" },
                Organizations = [

                    new() { Id = Guid.NewGuid(), Name = "org1" },
                    new() { Id = Guid.NewGuid(), Name = "org2" }
                ]
            };
            _authService.Setup(aus => aus.GetCurrentUserAsync()).ReturnsAsync(userId);
            _userRepository.Setup(repo => repo.GetBySpec(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<Expression<Func<User, object>>[]>()))
                .ReturnsAsync(sampleUser);

            var handler = new GetLoggedInUserDetailsQueryHandler(_authService.Object, _userRepository.Object, _mapper);
            var result = await handler.Handle(new GetLoggedInUserDetailsQuery(), CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(userId, result.Id);
            Assert.Contains(sampleUser.FirstName, result.FullName);
            Assert.NotNull(result.Profile);
            Assert.NotNull(result.Organizations);
        }
    }
}
