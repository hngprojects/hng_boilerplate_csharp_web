using Microsoft.Extensions.DependencyInjection;

namespace Hng.Graphql
{
    public static class ConfigureGraphql
    {
        public static IServiceCollection AddGraphql(this IServiceCollection services)
        {
            services.AddGraphQLServer()
                .AddQueryType<Queries>()
                .AddMutationType<Mutations>()
                .AddAuthorization();

            return services;
        }
    }
}
