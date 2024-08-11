using System.ComponentModel.DataAnnotations;
using Hng.Domain.Enums;

namespace Hng.Domain.Entities;

public class OrganizationInvite : EntityBase
{
    [Required]
    public Guid OrganizationId { get; set; }

    [Required]
    public string Email { get; set; }

    [Required]
    public OrganizationInviteStatus Status { get; set; } = OrganizationInviteStatus.Pending;

    [Required]
    public string InviteLink { get; set; }

    [Required]
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

    [Required]
    public DateTimeOffset ExpiresAt { get; set; } = DateTimeOffset.UtcNow.AddDays(7);

    public DateTimeOffset AcceptedAt { get; set; }

}

