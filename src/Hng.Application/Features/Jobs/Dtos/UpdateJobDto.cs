using Hng.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Hng.Application.Features.Jobs.Dtos
{
    public class UpdateJobDto
    {
        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("location")]
        public string Location { get; set; }

        [JsonPropertyName("salary_range")]
        public double Salary { get; set; }

        [JsonPropertyName("experience_level")]
        public ExperienceLevel? Level { get; set; }

        [JsonPropertyName("company_name")]
        public string Company { get; set; }
    }
}
