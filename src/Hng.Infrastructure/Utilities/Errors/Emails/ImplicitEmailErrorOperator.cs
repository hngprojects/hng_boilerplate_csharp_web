
namespace Hng.Infrastructure.Utilities.Errors.Emails;

public class ImplicitEmailErrorOperator(string Message) : Error(Message)
{
    public static implicit operator Result<string>(ImplicitEmailErrorOperator error)
    {
        
        return Result<string>.Failure(error);
        
    }
}
