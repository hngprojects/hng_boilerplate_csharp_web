using FluentValidation;
using Hng.Application.Exceptions;
using Hng.Application.Services;
using Hng.Domain.Repository;
using Hng.Infrastructure.Context;
using Hng.Infrastructure.Implementation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MySqlConnector;

namespace Hng.Web.Services
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddConfiguredServices(this IServiceCollection services, IConfiguration Configuration)
        {
            var smtpServer = Configuration["Smtp:Server"];
            var smtpPort = int.Parse(Configuration["Smtp:Port"]);
            var smtpUser = Configuration["Smtp:Username"];
            var smtpPass = Configuration["Smtp:Password"];

            services.AddSingleton<IEmailService>(new EmailService(smtpServer, smtpPort, smtpUser, smtpPass));

            services.AddScoped<DbContext, MyDBContext>();
            var connectionString = Configuration.GetConnectionString("DefaultConnectionString");

            services.AddDbContext<MyDBContext>(options =>
                options.UseSqlServer(connectionString));

            //services.AddTransient<IEmailService, EmailService>();
            services.AddScoped<IWaitlistService, WaitlistService>();
            services.AddScoped<IRateLimitRepository, RateLimitRepository>();
            services.AddScoped<IWaitlistUserRepository, WaitlistUserRepository>();

            services.AddValidatorsFromAssemblyContaining<WaitlistUserRequestModelValidator>();


            return services;
        }
    }
}