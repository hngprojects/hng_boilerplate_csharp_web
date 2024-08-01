using Hng.Domain.Common;

namespace Hng.Infrastructure.Utilities.Errors.OrganisationInvite;

public class OrganisationDoesNotExistError : ImplicitErrorOperator
{
    private OrganisationDoesNotExistError(string Message) : base(Message) { }

    public static OrganisationDoesNotExistError FromId(Guid id)
    {
        return new OrganisationDoesNotExistError($"No organisation with {id} exists");
    }
}

