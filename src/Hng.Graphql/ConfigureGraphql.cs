using Hng.Graphql.Features;
using Hng.Graphql.Features.Roles;
using Hng.Graphql.Features.UserManagement;
using HotChocolate.Execution.Configuration;

namespace Hng.Graphql
{
    public static class ConfigureGraphql
    {
        public static IServiceCollection RegisterFeatures(this IServiceCollection services)
        {
            services
                .AddScoped<UserManagementQueries>()
                .AddScoped<UserManagementMutations>()
                .AddScoped<RolesQueries>()
                .AddScoped<RolesMutations>();
            return services;
        }

        public static IRequestExecutorBuilder AddGraphQueries(this IRequestExecutorBuilder builder)
        {
            builder
                .AddQueryType<Queries>();
            return builder;
        }

        public static IRequestExecutorBuilder addGraphMutations(this IRequestExecutorBuilder builder)
        {
            builder
                .AddMutationType<Mutations>();
            return builder;
        }
    }
}
