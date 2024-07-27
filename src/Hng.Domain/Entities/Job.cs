using Hng.Domain.Enums;

namespace Hng.Domain.Entities;

public class Job : EntityBase
{
    public string Title { get; set; }
    public string Description { get; set; }
    public string Location { get; set; }
    public double Salary { get; set; }
    public ExperienceLevel Level { get; set; }
    public string Company { get; set; }
    public DateTime DatePosted { get; set; }
}