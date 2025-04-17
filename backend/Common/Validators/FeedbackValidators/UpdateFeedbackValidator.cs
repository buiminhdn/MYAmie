using Common.DTOs.FeedbackDtos;
using FluentValidation;
using Utility.Constants;

namespace Common.Validators.FeedbackValidators;
public class UpdateFeedbackValidator : AbstractValidator<UpdateFeedbackParams>
{
    public UpdateFeedbackValidator()
    {
        RuleFor(x => x.Id)
           .GreaterThan(0)
           .WithMessage(string.Format(ValidationMessages.InvalidInput, "Id"));

        RuleFor(x => x.Rating)
            .InclusiveBetween(1, 5)
            .WithMessage(string.Format(ValidationMessages.InvalidInput, "Rating"));

        RuleFor(x => x.Content)
            .NotEmpty()
            .WithMessage(string.Format(ValidationMessages.RequiredField, "Content"))
            .MaximumLength(ValidationLengths.Fields.Content)
            .WithMessage(string.Format(ValidationMessages.MaxLengthExceeded, "Content", ValidationLengths.Fields.Content));
    }
}
