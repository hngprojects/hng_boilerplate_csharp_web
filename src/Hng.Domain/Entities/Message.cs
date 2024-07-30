using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text;
using Hng.Domain.Enums;

namespace Hng.Domain.Entities;

public class Message : EntityBase
{
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
}
