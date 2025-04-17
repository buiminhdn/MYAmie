using Common.DTOs.EmailDtos;
using FluentValidation;
using Utility.Constants;

namespace Common.Validators.AccountValidators;
public class ResetPasswordValidator : AbstractValidator<ResetPasswordParams>
{
    public ResetPasswordValidator()
    {
        RuleFor(x => x.Email)
         .NotEmpty().WithMessage(string.Format(ValidationMessages.RequiredField, "Email"))
         .EmailAddress().WithMessage(string.Format(ValidationMessages.InvalidInput, "Email"));

        RuleFor(x => x.Code)
            .NotEmpty().WithMessage(string.Format(ValidationMessages.RequiredField, "Mã xác nhận"));

        RuleFor(x => x.NewPassword)
            .NotEmpty().WithMessage(string.Format(ValidationMessages.RequiredField, "Mật khẩu"))
            .MinimumLength(6).WithMessage(string.Format(ValidationMessages.MinLengthRequired, "Mật khẩu", 6));
    }
}
