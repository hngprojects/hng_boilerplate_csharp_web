using System.ComponentModel.DataAnnotations;

namespace Hng.Domain.Entities;

public class Squeeze
{
    [Required]
    public string FirstName { get; set; }

    [Required]
    public string LastName { get; set; }

    [EmailAddress]
    public string Email { get; set; }
}