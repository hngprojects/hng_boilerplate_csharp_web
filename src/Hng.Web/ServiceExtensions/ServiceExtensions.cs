using Hng.Domain.Entities.Models;
using Hng.Infrastructure.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Hng.Web.Services
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddConfiguredServices(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<MyDBContext>(options => options.UseNpgsql(connectionString));
            services.AddIdentityCore<User>(options =>
            {
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 2;
                options.Password.RequiredUniqueChars = 0;
                options.Password.RequireUppercase = false;
                options.Password.RequireDigit = false;
            })
                .AddRoles<IdentityRole<long>>().AddEntityFrameworkStores<MyDBContext>();

            return services;
        }
    }
}