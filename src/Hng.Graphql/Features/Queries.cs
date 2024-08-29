using Hng.Graphql.Features.Roles;
using Hng.Graphql.Features.UserManagement;

namespace Hng.Graphql.Features
{
    public class Queries
    {
        public UserManagementQueries UserManagement { get; }
        public RolesQueries Roles { get; }

        public Queries(UserManagementQueries userManagement, RolesQueries rolesQueries)
        {
            UserManagement = userManagement;
            Roles = rolesQueries;
        }
    }
}
