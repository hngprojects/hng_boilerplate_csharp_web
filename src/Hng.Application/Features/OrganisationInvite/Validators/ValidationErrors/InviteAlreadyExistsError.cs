using Hng.Domain.Entities;
using Hng.Infrastructure.Utilities.Results;

namespace Hng.Application.Features.OrganisationInvite.Validators.ValidationErrors;
public class InviteAlreadyExistsError : Error
{
    private InviteAlreadyExistsError(string Message) : base(Message) { }

    public static InviteAlreadyExistsError FromEmail(string email)
    {
        return new InviteAlreadyExistsError($"An invite already exists for the email {email}");
    }

    public static implicit operator Result<OrganizationInvite>(InviteAlreadyExistsError error) => Result<OrganizationInvite>.Failure(error);
}

