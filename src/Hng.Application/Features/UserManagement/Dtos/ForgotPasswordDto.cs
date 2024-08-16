using CSharpFunctionalExtensions;
using MediatR;
using System.Text.Json.Serialization;

namespace Hng.Application.Features.UserManagement.Dtos
{
    public record ForgotPasswordDto : IRequest<Result<ForgotPasswordResponse>>
    {
        public ForgotPasswordDto(string email)
        {
            Email = email;
        }

        public string Email { get; set; }
    }

    public record ForgotPasswordResponse
    {
        [JsonPropertyName("message")]
        public string Message { get; set; }

        [JsonPropertyName("status_code")]
        public int StatusCode { get; set; }

        [JsonPropertyName("data")]
        public ForgotPasswordData Data { get; set; }
    }

    public record ForgotPasswordData
    {
        [JsonPropertyName("message")]
        public string Message { get; set; }
    }
}
