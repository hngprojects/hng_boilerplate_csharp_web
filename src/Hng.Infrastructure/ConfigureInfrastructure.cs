using Hng.Infrastructure.Context;
using Hng.Infrastructure.Repository;
using Hng.Infrastructure.Repository.Interface;
using Hng.Infrastructure.Services;
using Hng.Infrastructure.Services.Interfaces;
using Hng.Infrastructure.Services.Internal;
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
            services.AddScoped<ITokenService, TokenService>();
            services.AddTransient<IGoogleAuthService, GoogleAuthService>();
            services.AddScoped<IPasswordService, PasswordService>();
            services.AddScoped<IOrganizationInviteService, OrganizationInviteService>();
            services.AddScoped<SeederService>();
            services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(connectionString));
            services.AddScoped<IMessageQueueService, MessageQueueService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddHostedService<MessageQueueHandlerService>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IFacebookAuthService, FacebookAuthService>();
            return services;
        }
    }

}