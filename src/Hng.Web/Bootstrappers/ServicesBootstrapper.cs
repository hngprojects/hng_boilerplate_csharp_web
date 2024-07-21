using Hng.Application.Interfaces;
using Hng.Application.Services;
using Hng.Domain.Models;
using Hng.Infrastructure.Repository.Interface;
using Hng.Infrastructure.Repository.Mock;
using Hng.Infrastructure.Repository.SqlData;
using Hng.Web.Mappers;

namespace Hng.Web.Bootstrappers
{
    public static class ServicesBootstrapper
    {
        /// <summary>
        /// Extension method that adds support for registering dependencies to IOC container
        /// </summary>
        public static IServiceCollection AddDependencies(this IServiceCollection services, AppSettings appSettings)
        {
            return AddDependenciesToServiceCollection(services, appSettings);
        }

        /// <summary>
        /// Extension method that adds support for registering dependencies to IOC container
        /// </summary>
        public static IServiceCollection AddDependenciesToServiceCollection(IServiceCollection services, AppSettings appSettings)
        {
            //add appsettings
            services.AddSingleton(appSettings);

            //add automapper
            services.AddAutoMapper(typeof(MappingProfile));
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            if (appSettings.Settings.UseMockForDatabase)
            {
                //mock data
                services.AddScoped<IUserRepository, MockUserRepository>();
            }
            else
            {
                //db data
                services.AddScoped<IUserRepository, UserRepository>();
            }

            //service registeration
            services.AddScoped<IUserService, UserService>();


            return services;
        }
    }
}
