using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Hng.Domain.Entities;

[Index(nameof(Name), IsUnique = true)]
public class EmailTemplate : EntityBase
{
    [MaxLength(100)]
    public string Name { get; set; }
    public string Subject { get; set; }
    public string TemplateBody { get; set; }
    public Dictionary<string, string> PlaceHolders { get; set; } = [];
}
