using CSharpFunctionalExtensions;
using MediatR;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Hng.Application.Features.UserManagement.Dtos
{
    public record VerifyForgotPasswordCodeDto : IRequest<Result<VerifyForgotPasswordCodeResponse>>
    {
        public VerifyForgotPasswordCodeDto(string email, string code)
        {
            Code = code;
            Email = email;
        }

        [Required(ErrorMessage = "Code is required")]
        public string Code { get; set; }

        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }
    }

    public record VerifyForgotPasswordCodeResponse
    {
        [JsonPropertyName("message")]
        public string Message { get; set; }

        [JsonPropertyName("status_code")]
        public int StatusCode { get; set; }

        [JsonPropertyName("data")]
        public VerifyForgotPasswordCodeData Data { get; set; }
    }

    public record VerifyForgotPasswordCodeData
    {
        [JsonPropertyName("message")]
        public string Message { get; set; }
    }
}
