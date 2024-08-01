using System.Reflection;
using Hng.Application.Features.PaymentIntegrations.Paystack.Services;
using Hng.Infrastructure.Services;
using Hng.Infrastructure.Utilities;
using Hng.Infrastructure.Utilities.StringKeys;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Hng.Application
{
    public static class ConfigureApplication
    {
        public static IServiceCollection AddApplicationConfig(this IServiceCollection services, IConfiguration configurations)
        {
            services.AddMediatR(cf => cf.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));

            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(jwtOptions =>
            {
                jwtOptions.TokenValidationParameters = TokenService.GetTokenValidationParameters(configurations);
            });
            //.AddGoogle(googleOptions =>
            //{
            //    googleOptions.ClientId = configurations["Authentication:Google:ClientId"];
            //    googleOptions.ClientSecret = configurations["Authentication:Google:ClientSecret"];
            //});

            services.AddAuthorization();

            services.AddHttpClient<IPaystackClient, PaystackClient>(c =>
            {
                c.BaseAddress = new(configurations.GetSection("PaystackApiKeys").Get<PaystackApiKeys>().Endpoint);
            });

            services.AddSingleton(configurations.GetSection("PaystackApiKeys").Get<PaystackApiKeys>());
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigins",
                    builder =>
                    {
                        builder.AllowAnyOrigin()
                               .AllowAnyMethod()
                               .AllowAnyHeader();
                    });
            });
            services.AddSingleton(configurations.GetSection("SmtpCredentials").Get<SmtpCredentials>());

            return services;
        }
    }
}