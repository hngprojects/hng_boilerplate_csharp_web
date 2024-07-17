using FluentValidation;
using Hng.Application.Exceptions;
using Hng.Application.Services;
using Hng.Domain.Repository;
using Hng.Infrastructure.Context;
using Hng.Infrastructure.Implementation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Hng.Web.Services
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddConfiguredServices(this IServiceCollection services, string connectionString)
        {
            services.AddScoped<DbContext, MyDBContext>();
            services.AddTransient<IEmailService, EmailService>();
            services.AddScoped<IWaitlistService, WaitlistService>();
            services.AddScoped<IRateLimitRepository, RateLimitRepository>();
            services.AddScoped<IWaitlistUserRepository, WaitlistUserRepository>();

            services.AddValidatorsFromAssemblyContaining<WaitlistUserRequestModelValidator>();

            services.AddDbContext<MyDBContext>(options =>
                options.UseMySQL(connectionString));

            return services;
        }
    }
}