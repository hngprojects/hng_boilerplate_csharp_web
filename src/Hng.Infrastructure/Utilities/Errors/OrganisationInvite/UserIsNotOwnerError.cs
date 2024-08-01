namespace Hng.Infrastructure.Utilities.Errors.OrganisationInvite;

public class UserIsNotOwnerError : ImplicitErrorOperator
{
    private UserIsNotOwnerError(string Message) : base(Message) { }

    public static UserIsNotOwnerError FromIds(Guid userId, Guid organizationId)
    {
        return new UserIsNotOwnerError($"The logged-in user is not the owner of the specified organization");
    }
}

