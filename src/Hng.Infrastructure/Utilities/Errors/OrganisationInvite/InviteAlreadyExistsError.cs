using Hng.Domain.Common;

namespace Hng.Infrastructure.Utilities.Errors.OrganisationInvite;

public class InviteAlreadyExistsError : ImplicitErrorOperator
{
    private InviteAlreadyExistsError(string Message) : base(Message) { }

    public static InviteAlreadyExistsError FromEmail(string email)
    {
        return new InviteAlreadyExistsError($"An invite already exists for {email}");
    }
}

