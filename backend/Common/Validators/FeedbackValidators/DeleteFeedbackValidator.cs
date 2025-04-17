using Common.DTOs.FeedbackDtos;
using FluentValidation;
using Utility.Constants;

namespace Common.Validators.FeedbackValidators;
public class DeleteFeedbackValidator : AbstractValidator<DeleteFeedbackParams>
{
    public DeleteFeedbackValidator()
    {
        RuleFor(x => x.Id)
           .GreaterThan(0)
           .WithMessage(string.Format(ValidationMessages.InvalidInput, "Id"));
    }
}
