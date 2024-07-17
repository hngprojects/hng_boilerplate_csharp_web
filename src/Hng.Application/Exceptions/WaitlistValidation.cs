using FluentValidation;
using Hng.Application.Models.WaitlistModels;


namespace Hng.Application.Exceptions
{
    public class WaitlistUserRequestModelValidator : AbstractValidator<WaitlistUserRequestModel>
    {
        public WaitlistUserRequestModelValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Invalid email format");

            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("FullName is required");
        }
    }

}
