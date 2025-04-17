using Common.DTOs.AuthDtos;
using FluentValidation;
using Utility.Constants;

namespace Common.Validators.AuthValidators;
public class SignInValidator : AbstractValidator<SignInParams>
{
    public SignInValidator()
    {
        RuleFor(x => x.Email)
           .NotEmpty().WithMessage(string.Format(ValidationMessages.RequiredField, "Email"))
           .EmailAddress().WithMessage(string.Format(ValidationMessages.InvalidInput, "Email"));

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage(string.Format(ValidationMessages.RequiredField, "Mật khẩu"));
    }
}
