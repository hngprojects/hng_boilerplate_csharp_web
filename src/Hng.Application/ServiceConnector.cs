using Hng.Application.Features.PaymentIntegrations.Paystack.Services;
using Hng.Infrastructure.Utilities.StringKeys;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Hng.Infrastructure.Services
{
    public static class ServiceConnector
    {
        public static void AddConfigurationSettings(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpClient<PaystackClient>(c =>
            {
                c.BaseAddress = new(configuration.GetSection("PaystackApiKeys").Get<PaystackApiKeys>().Endpoint);
            });

            services.AddSingleton(configuration.GetSection("PaystackApiKeys").Get<PaystackApiKeys>());
        }
    }
}
