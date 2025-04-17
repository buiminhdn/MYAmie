using Common.DTOs.Core;
using FluentValidation;
using Utility.Constants;

namespace Common.Validators.Core;
public class BaseParamsValidator : AbstractValidator<BaseParams>
{
    public BaseParamsValidator()
    {
        RuleFor(x => x.CurrentUserId)
            .NotEmpty().WithMessage(ValidationMessages.PleaseLogin);

        RuleFor(x => x.CurrentUserRole)
            .NotEmpty().WithMessage(ValidationMessages.PleaseLogin);
    }
}
