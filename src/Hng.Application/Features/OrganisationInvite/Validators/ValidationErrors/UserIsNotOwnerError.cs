
namespace Hng.Application.Features.OrganisationInvite.Validators.ValidationErrors;
public class UserIsNotOwnerError : ImplicitErrorOperator
{
    private UserIsNotOwnerError(string Message) : base(Message) { }

    public static UserIsNotOwnerError FromIds(Guid userId, Guid organizationId)
    {
        return new UserIsNotOwnerError($"You are not the owner of the specified organization");
    }
}

