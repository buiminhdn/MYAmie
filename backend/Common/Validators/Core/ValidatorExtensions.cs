using FluentValidation.Results;

namespace Common.Validators.Core;
public static class ValidatorExtensions
{
    public static string GetErrorMessages(this ValidationResult validationResult)
    {
        if (validationResult.Errors.Count > 0)
        {
            return string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
        }
        return string.Empty;
    }
}