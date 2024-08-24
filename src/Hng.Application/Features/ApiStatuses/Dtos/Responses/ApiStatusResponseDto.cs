using Hng.Application.Features.ApiStatuses.Dtos.Requests;
using System.Text.Json.Serialization;

namespace Hng.Application.Features.ApiStatuses.Dtos.Responses
{
    public record ApiStatusResponseDto : ApiStatusModel
    {
        [JsonPropertyName("last_checked")]
        public DateTime LastChecked { get; set; }
    }
}