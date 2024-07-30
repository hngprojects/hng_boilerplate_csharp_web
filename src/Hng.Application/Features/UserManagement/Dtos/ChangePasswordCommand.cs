using CSharpFunctionalExtensions;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Hng.Application.Features.UserManagement.Dtos
{
    public record ChangePasswordCommand : IRequest<Result<ChangePasswordResponse>>
    {
        [Required(ErrorMessage = "Old Password is required")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "New Password is required")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long")]
        [RegularExpression(@"^(?=.*[a-zA-Z])(?=.*\d).{8,}$",
            ErrorMessage = "Password must contain at least one letter and one number")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Confirm Password is required")]
        [Compare("NewPassword")]
        public string ConfirmNewPassword { get; set; }
    }

    public record ChangePasswordResponse
    {
        public string Message { get; set; }
    }
}
