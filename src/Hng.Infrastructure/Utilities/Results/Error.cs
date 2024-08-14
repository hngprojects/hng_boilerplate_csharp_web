namespace Hng.Infrastructure.Utilities.Results;

public class Error(string Message)
{
    public string Message { get; set; } = Message;

}
