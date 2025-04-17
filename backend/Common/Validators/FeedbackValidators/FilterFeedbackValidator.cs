using FluentValidation;
using MYAmie.Common.DTOs.FeedbackDtos;
using Utility.Constants;

namespace Common.Validators.FeedbackValidators;
public class FilterFeedbackValidator : AbstractValidator<FilterFeedbackParams>
{
    public FilterFeedbackValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage(string.Format(ValidationMessages.InvalidInput, "Id"));
    }
}
