
using FluentValidation.Results;

namespace Common.Validators.Core;
public interface IValidationFactory
{
    ValidationResult Validate<T>(T data);
    Task<ValidationResult> ValidateAsync<T>(T data);
}
