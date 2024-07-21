using Hng.Application.Test.Mocks;
using Hng.Domain.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NUnit.Framework;

namespace Hng.Application.Test
{
    public class BaseTest
    {
        public static ServiceProvider ServiceProvider;

        protected string UserName { get; } = "johndoe";
        protected Guid ProfileId { get; set; } = new Guid("7acbba30-a989-4aa4-c702-08db3920bd4e");

        static BaseTest()
        {
            ServiceProvider = SetupDependencies();
        }

        public BaseTest()
        {
        }

        [SetUp]
        public void Init()
        {
            //some tests need affect static mock data need to be reset before each test
            ServiceProvider = SetupDependencies();
        }

        private static ServiceProvider SetupDependencies()
        {
            var appSettings = new AppSettings
            {
                Settings = new AppSettings.SettingsObject
                {
                    InTestMode = true,
                    UseMockForDatabase = true
                }
            };

            IServiceCollection serviceCollection = new ServiceCollection();
            serviceCollection = Hng.Web.Bootstrappers.ServicesBootstrapper.AddDependenciesToServiceCollection(serviceCollection, appSettings);

            serviceCollection.AddScoped(typeof(ILogger<>), typeof(MockLogger<>));

            return serviceCollection.BuildServiceProvider();
        }
    }
}
