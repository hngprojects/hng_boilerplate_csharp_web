using Hng.Domain.Common;
using Hng.Domain.Entities;

namespace Hng.Infrastructure.Utilities.Errors.OrganisationInvite;

public class ImplicitErrorOperator(string Message) : Error(Message)
{
    public static implicit operator Result<OrganizationInvite>(ImplicitErrorOperator error)
    {
        return Result<OrganizationInvite>.Failure(error);
    }
}
