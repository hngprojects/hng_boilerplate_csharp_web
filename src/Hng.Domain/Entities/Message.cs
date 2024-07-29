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
    public string Recipient { get; set; }

    [Required]
    public string Content { get; set; }

    [Required]
    public MessageStatus Status { get; set; } = MessageStatus.Pending;

    [Required]
    public int RetryCount { get; set; }

    public DateTimeOffset CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTimeOffset? LastAttemptedAt { get; set; } = null;

    public override string ToString()
    {
        FieldInfo[] fields = GetType().GetFields();

        StringBuilder stringBuilder = new();

        foreach (var field in fields)
        {
            System.Console.WriteLine("tostring()");
            stringBuilder.AppendLine(field.Name + "     : " + field.GetValue(field));
        }
        return stringBuilder.ToString();
    }
}
