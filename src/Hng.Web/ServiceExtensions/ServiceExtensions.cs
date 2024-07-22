using Hng.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Hng.Web.Services
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddConfiguredServices(this IServiceCollection services,
            string connectionString)
        {
            services.AddScoped<DbContext, MyDBContext>();

            services.AddDbContext<MyDBContext>(options =>
                options.UseNpgsql(connectionString));

            return services;
        }
    }
}
