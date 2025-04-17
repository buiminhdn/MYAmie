using Common.DTOs.AccountDtos;
using FluentValidation;
using Utility.Constants;

namespace Common.Validators.AccountValidators;
public class ChangePasswordValidator : AbstractValidator<ChangePasswordParams>
{
    public ChangePasswordValidator()
    {
        RuleFor(x => x.OldPassword)
           .NotEmpty().WithMessage(string.Format(ValidationMessages.RequiredField, "Mật khẩu cũ"));

        RuleFor(x => x.NewPassword)
            .NotEmpty().WithMessage(string.Format(ValidationMessages.RequiredField, "Mật khẩu mới"))
            .MinimumLength(6).WithMessage(string.Format(ValidationMessages.MinLengthRequired, "Mật khẩu", 6));
    }
}
