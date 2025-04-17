using Common.DTOs.EmailDtos;
using FluentValidation;
using Utility.Constants;

namespace Common.Validators.EmailValidators;
public class VerifyEmailValidator : AbstractValidator<VerifyEmailParams>
{
    public VerifyEmailValidator()
    {
        RuleFor(x => x.Email)
           .NotEmpty().WithMessage(string.Format(ValidationMessages.RequiredField, "Email"))
           .EmailAddress().WithMessage(string.Format(ValidationMessages.InvalidInput, "Email"));

        RuleFor(x => x.Code)
           .NotEmpty().WithMessage(string.Format(ValidationMessages.RequiredField, "Mã xác thực"));
    }
}
