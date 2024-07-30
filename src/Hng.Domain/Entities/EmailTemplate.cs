namespace Hng.Domain.Entities;

public class EmailTemplate : EntityBase
{
    public string Name { get; set; }
    public string Subject { get; set; }
    public string TemplateBody { get; set; }
    public Dictionary<string, string> PlaceHolders { get; set; } = [];
}
