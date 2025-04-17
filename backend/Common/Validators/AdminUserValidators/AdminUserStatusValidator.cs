using Common.DTOs.AdminUserDtos;
using FluentValidation;
using Utility.Constants;

namespace Common.Validators.AdminUserValidators;
public class AdminUserStatusValidator : AbstractValidator<AdminUserStatusParams>
{
    public AdminUserStatusValidator()
    {
        RuleFor(x => x.UserId)
            .GreaterThan(0)
            .WithMessage(string.Format(ValidationMessages.InvalidInput, "Id"));

        RuleFor(x => x.Status)
            .IsInEnum()
            .WithMessage(string.Format(ValidationMessages.InvalidInput, "Trạng thái"));
    }
}
