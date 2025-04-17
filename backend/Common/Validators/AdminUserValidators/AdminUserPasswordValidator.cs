using Common.DTOs.AdminUserDtos;
using FluentValidation;
using Utility.Constants;

namespace Common.Validators.AdminUserValidators;
public class AdminUserPasswordValidator : AbstractValidator<AdminUserPasswordParams>
{
    public AdminUserPasswordValidator()
    {
        RuleFor(x => x.UserId)
            .GreaterThan(0)
            .WithMessage(string.Format(ValidationMessages.InvalidInput, "Id"));

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage(string.Format(ValidationMessages.RequiredField, "Mật khẩu"))
            .MinimumLength(6).WithMessage(string.Format(ValidationMessages.MinLengthRequired, "Mật khẩu", 6));
    }
}
