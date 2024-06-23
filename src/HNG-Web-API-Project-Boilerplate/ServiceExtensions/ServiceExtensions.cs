using HNG.Boilerplate.Infrastructure.Context;
using HNG_Boilerplate_Application.Services;
using HNG_Boilerplate_Domain.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace HNG_Web_API_Project_Boilerplate.Services
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddConfiguredServices(this IServiceCollection services, string connectionString)
        {
            services.AddScoped<DbContext, MyDBContext>();

            services.AddDbContext<MyDBContext>(options =>
                options.UseMySQL(connectionString));

            services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            // Application Services
            services.AddTransient<IProductService, ProductService>();

            

            return services;
        }


    }
}
