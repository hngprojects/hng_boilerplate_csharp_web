using Hng.Application.Services.Implementation;
using Hng.Application.Services.Interfaces;
using Hng.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Hng.Web.Services
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddConfiguredServices(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<MyDBContext>(options => options.UseNpgsql(connectionString));
            services.AddScoped<IUserService, UserService>();
            return services;
        }
    }
}