namespace Hng.Infrastructure.Utilities.EmailQueue;

public record Email(string RecipientName, string RecipientMailAddress, string Subject, string Content);



