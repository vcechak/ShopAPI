using System.ComponentModel.DataAnnotations;

namespace Shop.Api.Validation;

public class EnumerableNotEmpty : ValidationAttribute
{
    protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null)
        {
            return new ValidationResult(ErrorMessage ?? "The list cannot be null.");
        }

        if (value is not IEnumerable<object> enumerable)
        {
            return new ValidationResult(ErrorMessage ?? "The value must be a list.");
        }

        if (!enumerable.GetEnumerator().MoveNext())
        {
            return new ValidationResult(ErrorMessage ?? "The list cannot be empty.");
        }

        return ValidationResult.Success!;
    }
}
