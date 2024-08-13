namespace Hng.Infrastructure.Utilities.Errors.Messages;

public class InvalidEmailError : ImplicitMessageErrorOperator
{
    private InvalidEmailError(string message) : base(message) { }

    public static InvalidEmailError FromEmail(string email)
    {
        return new InvalidEmailError(EmailErrors.InvalidEmailError(email));
    }
}
