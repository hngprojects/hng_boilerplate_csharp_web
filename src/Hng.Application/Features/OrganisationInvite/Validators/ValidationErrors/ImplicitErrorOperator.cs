using Hng.Domain.Entities;
using Hng.Infrastructure.Utilities.Results;

namespace Hng.Application.Features.OrganisationInvite.Validators.ValidationErrors;

public class ImplicitErrorOperator(string Message) : Error(Message)
{
    public static implicit operator Result<Organization>(ImplicitErrorOperator error)
    {
        return Result<Organization>.Failure(error);
    }
}
