namespace Shop.DataAccess.Validation;

using System.ComponentModel.DataAnnotations;
using System.Reflection;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class AtLeastOneRequired : ValidationAttribute
{
    private readonly string[] _propertyNames;

    public AtLeastOneRequired(params string[] propertyNames)
    {
        _propertyNames = propertyNames;
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var type = validationContext.ObjectType;

        foreach (var propName in _propertyNames)
        {
            var prop = type.GetProperty(propName, BindingFlags.Public | BindingFlags.Instance);
            if (prop == null)
            {
                return new ValidationResult($"Property '{propName}' does not exist.");
            }

            var propValue = prop.GetValue(validationContext.ObjectInstance) as string;
            if (!string.IsNullOrEmpty(propValue))
            {
                return ValidationResult.Success;
            }
        }

        var joinedNames = string.Join(", ", _propertyNames);
        return new ValidationResult(ErrorMessage ?? $"At least one of the following must be provided: {joinedNames}.");
    }
}