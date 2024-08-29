using System.ComponentModel.DataAnnotations;
using Hng.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Hng.Domain.Entities;

[Index(nameof(InviteCode))]
public class OrganizationInvite : EntityBase
{
    [Required]
    public Guid OrganizationId { get; set; }

    [Required]
    public string Email { get; set; }

    [Required]
    public OrganizationInviteStatus Status { get; set; } = OrganizationInviteStatus.Pending;

    [Required]
    public Guid InviteCode { get; set; }

    [Required]
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

    [Required]
    public DateTimeOffset ExpiresAt { get; set; } = DateTimeOffset.UtcNow.AddDays(7);

    public DateTimeOffset AcceptedAt { get; set; }

}

