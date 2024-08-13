using System.ComponentModel.DataAnnotations;

namespace Hng.Application.Shared.Dtos.Validators;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
public class EmailCollectionAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value is not IEnumerable<string> emails)
        {
            return new ValidationResult("Emails should not be null.");
        }

        var emailValidator = new EmailAddressAttribute();
        List<ValidationResult> failedValidationResults = [];
        foreach (var email in emails)
        {
            if (!emailValidator.IsValid(email))
            {
                failedValidationResults.Add(new ValidationResult($"The email '{email}' is not a valid email address"));
            }
        }

        return failedValidationResults.Count != 0 ? new ValidationResult(string.Join($",   ", failedValidationResults)) : ValidationResult.Success;

    }
};
