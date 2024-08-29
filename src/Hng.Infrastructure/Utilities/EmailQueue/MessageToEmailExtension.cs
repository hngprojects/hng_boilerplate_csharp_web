using Hng.Domain.Entities;

namespace Hng.Infrastructure.Utilities.EmailQueue;

public static class MessageToEmailExtension
{
    public static Email ToEmail(this Message message)
    {
        return new Email(message.RecipientName, message.RecipientContact, message.Subject, message.Content);
    }

    public static Message ToMessage(this Email email)
    {
        return Message.CreateEmail(email.RecipientMailAddress, email.Subject, email.Content, email.RecipientName);
    }
}
