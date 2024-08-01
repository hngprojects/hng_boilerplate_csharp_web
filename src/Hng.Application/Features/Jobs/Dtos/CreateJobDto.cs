using System.Text.Json.Serialization;
using Hng.Domain.Enums;

namespace Hng.Application.Features.Jobs.Dtos;

public class CreateJobDto
{
    [JsonPropertyName("title")]
    public string Title { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("location")]
    public string Location { get; set; }

    [JsonPropertyName("salary")]
    public double Salary { get; set; }

    [JsonPropertyName("level")]
    public ExperienceLevel Level { get; set; }

    [JsonPropertyName("company")]
    public string Company { get; set; }
}