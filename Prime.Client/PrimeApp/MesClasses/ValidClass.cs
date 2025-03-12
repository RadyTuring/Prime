

using System;
using System.ComponentModel.DataAnnotations;
namespace CustomValidations;
public class RequiredIfNotZeroAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var intValue = value as int?;

        // Check if the value is null or not zero
        if (!intValue.HasValue || intValue.Value == 0)
        {
            return new ValidationResult(ErrorMessage ?? "Please Choose the Update level");
        }

        return ValidationResult.Success;
    }
}


