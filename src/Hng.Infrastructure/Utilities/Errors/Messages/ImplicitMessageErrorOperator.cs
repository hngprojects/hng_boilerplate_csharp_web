using Hng.Domain.Entities;

namespace Hng.Infrastructure.Utilities.Errors.Messages;

public class ImplicitMessageErrorOperator(string Message) : Error(Message)
{
    public static implicit operator Result<Message>(ImplicitMessageErrorOperator error)
    {
        
        return Result<Message>.Failure(error);
        
    }
}
