namespace Hng.Infrastructure.Utilities;

public static class EmailErrors
{
    public static string InvalidEmailError(string email) => $"The email {email} is not valid";
}
