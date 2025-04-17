using Common.DTOs.AdminUserDtos;
using FluentValidation;
using Utility.Constants;

namespace Common.Validators.AdminUserValidators;
public class AddMarketingEmailValidator : AbstractValidator<AddMarketingEmailParams>
{
    public AddMarketingEmailValidator()
    {
        RuleFor(x => x.Subject)
            .NotEmpty().WithMessage(string.Format(ValidationMessages.RequiredField, "Tiêu đề"));

        RuleFor(x => x.Body)
            .NotEmpty().WithMessage(string.Format(ValidationMessages.RequiredField, "Nội dung"));
    }
}
