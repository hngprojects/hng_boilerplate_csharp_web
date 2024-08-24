using System.ComponentModel.DataAnnotations;
using Hng.Domain.Enums;

namespace Hng.Domain.Entities;

public class Message : EntityBase
{
    private Message(MessageType type, string recipientContact, string recipientName, string subject, string content)
    {
        Id = Guid.NewGuid();
        Type = type;
        RecipientName = recipientName;
        RecipientContact = recipientContact;
        Subject = subject;
        Content = content;
    }
    [Required]
    public MessageType Type { get; set; }

    [Required]
    public string RecipientName { get; set; }

    [Required]
    public string RecipientContact { get; set; }

    [Required]
    public string Subject { get; set; }

    [Required]
    public string Content { get; set; }

    [Required]
    public MessageStatus Status { get; set; } = MessageStatus.Pending;

    [Required]
    public int RetryCount { get; set; } = 0;

    [Required]
    public DateTimeOffset CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTimeOffset? LastAttemptedAt { get; set; } = null;

    public static Message CreateEmail(string recipientContact, string subject, string content, string recipientName = "You")
    {
        return new Message(MessageType.Email, recipientContact, recipientName, subject, content);
    }
}
