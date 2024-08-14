using System.ComponentModel.DataAnnotations;

namespace Hng.Application.Shared.Validators;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
public class ValidGuidAttribute : ValidationAttribute
{
    public bool AllowEmpty { get; set; } = false;

    public override bool IsValid(object value)
    {
        if (value == null)
        {
            return !AllowEmpty;
        }

        if (value is Guid guid)
        {
            return AllowEmpty || guid != Guid.Empty;
        }

        if (value is string stringValue)
        {
            return Guid.TryParse(stringValue, out Guid parsedGuid) && (AllowEmpty || parsedGuid != Guid.Empty);
        }

        return false;
    }

    public override string FormatErrorMessage(string name)
    {
        return $"The field must be a valid GUID" + (AllowEmpty ? "" : " and cannot be empty");
    }
}
