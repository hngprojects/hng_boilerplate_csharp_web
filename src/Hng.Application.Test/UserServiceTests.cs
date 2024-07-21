using Hng.Application.Interfaces;
using Hng.Application.Test.Builders;
using NUnit.Framework;

namespace Hng.Application.Test
{
    public class UserServiceTests : BaseTest
    {
        [Test]
        public async Task User_GetByValidId_IsSuccessful()
        {
            //arrange
            var service = Build();

            var userId = new Guid("7acbba30-a989-4aa4-c702-08db3920bd4e");

            //act
            var actual = await service.GetUserByIdAsync(userId);

            //assert
            Assert.Multiple(() =>
            {
                Assert.That(actual, Is.Not.Null);
                Assert.That(actual.id, Is.EqualTo(ProfileId));
            });
        }

        [Test]
        public async Task User_GetByInvalidId_ShouldFail_Successful()
        {
            //arrange
            var service = Build();

            //invalid user id - not registred in system
            var userId = new Guid("8CA18E11-FE42-461D-9282-3CC5ABAB3CA7");

            //act
            var actual = await service.GetUserByIdAsync(userId);

            //assert
            Assert.That(actual, Is.Null);
        }


        public IUserService Build()
        {
            return DefaultServiceBuilder.Build<IUserService>();
        }
    }
}
