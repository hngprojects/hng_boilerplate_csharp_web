using Hng.Application.Features.Subscriptions.Dtos.Requests;
using Hng.Application.Features.Subscriptions.Dtos.Responses;
using System.Text.Json.Serialization;

namespace Hng.Application.Features.UserManagement.Dtos
{
    public class SignUpResponse
    {
        [JsonPropertyName("message")]
        public string Message { get; set; }
        public SignupResponseData Data { get; set; }
        [JsonPropertyName("access_token")]
        public string Token { get; set; }
        [JsonPropertyName("status_code")]
        public int StatusCode { get; set; }
    }

    public class SignupResponseData
    {
        [JsonPropertyName("user")]
        public UserResponseDto User { get; set; }
        [JsonPropertyName("organisations")]
        public List<OrganisationDto> Organization { get; set; } = [];
        [JsonPropertyName("subscriptions")]
        public List<SubscribeFreePlanResponse> Subscription { get; set; } = [];
    }
}
