
namespace Hng.Application.Features.OrganisationInvite.Validators.ValidationErrors;

public class OrganisationDoesNotExistError : ImplicitErrorOperator
{
    private OrganisationDoesNotExistError(string Message) : base(Message) { }

    public static OrganisationDoesNotExistError FromId(Guid id)
    {
        return new OrganisationDoesNotExistError($"No organisation exists for the id {id}");
    }
}

