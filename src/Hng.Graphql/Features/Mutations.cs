using Hng.Graphql.Features.Roles;
using Hng.Graphql.Features.UserManagement;

namespace Hng.Graphql.Features
{
    public class Mutations
    {
        public UserManagementMutations UserManagement { get; }
        public RolesMutations Roles { get; }

        public Mutations(UserManagementMutations userManagement, RolesMutations rolesMutations)
        {
            UserManagement = userManagement;
            Roles = rolesMutations;
        }
    }
}
