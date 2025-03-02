using FluentValidation;
using Hng.Application.Features.UserManagement.Dtos;
using MediatR;

namespace Hng.Application.Features.UserManagement.Commands
{
    public class CreateUserLoginCommand : AbstractValidator<CreateUserLoginCommand>, IRequest<UserLoginResponseDto<SignupResponseData>>
    {
        public CreateUserLoginCommand(UserLoginRequestDto loginRequest)
        {
            LoginRequestBody = loginRequest;

            RuleFor(x => x.LoginRequestBody.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");

            RuleFor(x => x.LoginRequestBody.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
                .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$")
                .WithMessage("Password must include an uppercase letter, a lowercase letter, a number, and a special character.");
        }

        public UserLoginRequestDto LoginRequestBody { get; }
    }

}
