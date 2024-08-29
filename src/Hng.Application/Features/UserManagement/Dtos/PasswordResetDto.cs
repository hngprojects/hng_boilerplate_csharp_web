using CSharpFunctionalExtensions;
using MediatR;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Hng.Application.Features.UserManagement.Dtos
{
    public record PasswordResetDto : IRequest<Result<PasswordResetResponse>>
    {
        [Required(ErrorMessage = "New Password is required")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long")]
        [RegularExpression(@"^(?=.*[a-zA-Z])(?=.*\d).{8,}$",
            ErrorMessage = "Password must contain at least one letter and one number")]
        [JsonPropertyName("new_password")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Confirm Password is required")]
        [Compare("NewPassword")]
        [JsonPropertyName("confirm_new_password")]
        public string ConfirmNewPassword { get; set; }
    }

    public record PasswordResetResponse
    {
        [JsonPropertyName("message")]
        public string Message { get; set; }

        [JsonPropertyName("status_code")]
        public int StatusCode { get; set; }

        [JsonPropertyName("data")]
        public PasswordResetData Data { get; set; }
    }

    public record PasswordResetData
    {
        [JsonPropertyName("message")]
        public string Message { get; set; }
    }
}
