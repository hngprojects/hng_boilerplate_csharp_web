using System.Reflection;
using Hng.Application.Features.ExternalIntegrations.FilesUploadIntegrations.Cloudinary.Services;
using Hng.Application.Features.ExternalIntegrations.PaymentIntegrations.Paystack.Services;
using Hng.Application.Features.OrganisationInvite.Validators;
using Hng.Application.Utils;
using Hng.Infrastructure.EmailTemplates;
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

            services.AddSingleton(configurations.GetSection("CloudinarySettings").Get<CloudinarySettings>());



            services.AddScoped<IImageService, ImageService>();
            services.AddScoped<IRequestValidator, RequestValidator>();

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

            services.AddSingleton(configurations.GetSection("EmailTemplateDirectory").Get<TemplateDir>());

            services.AddOptions<FrontendUrl>()
                .Bind(configurations.GetSection("FrontendUrl"))
                .ValidateDataAnnotations()
                .ValidateOnStart();

            return services;
        }
    }
}