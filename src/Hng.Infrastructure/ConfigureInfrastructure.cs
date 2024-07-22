using Hng.Infrastructure.Context;
using Hng.Infrastructure.Repository;
using Hng.Infrastructure.Repository.Interface;
using Hng.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Hng.Infrastructure
{
    public static class ConfigureInfrastructure
    {
        public static IServiceCollection AddInfrastructureConfig(this IServiceCollection services, string connectionString)
        {
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<DbContext, ApplicationDbContext>();
            services.AddScoped<SeederService>();
            services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(connectionString));

            return services;
        }
    }
}