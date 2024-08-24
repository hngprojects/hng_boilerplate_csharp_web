using Hng.Domain.Enums;
using System.Text.Json.Serialization;

namespace Hng.Application.Features.ApiStatuses.Dtos.Requests
{
    public record UpdateApiStatusDto
    {
        public List<ApiStatusModel> ApiStatusModels { get; set; } = [];
    }

    public record ApiStatusModel
    {
        [JsonPropertyName("api_group")]
        public string ApiGroup { get; set; }

        [JsonPropertyName("status")]
        public ApiStatusType Status { get; set; }

        [JsonPropertyName("response_time")]
        public long ResponseTime { get; set; }

        [JsonPropertyName("details")]
        public string Details { get; set; }
    }

    public record CreateApiStatusResponseDto
    {
        [JsonPropertyName("message")]
        public string Message { get; set; }

        [JsonPropertyName("status_code")]
        public int StatusCode { get; set; }
    }
}