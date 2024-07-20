using Hng.Infrastructure.Context;
using Hng.Infrastructure.Repository;
using Hng.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

namespace Hng.Web.Services
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddConfiguredServices(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<MyDBContext>(options => options.UseNpgsql(connectionString));
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<UserRepository>();
            return services;
        }
    }
}