using Common.DTOs.FeedbackDtos;
using FluentValidation;
using Utility.Constants;

namespace Common.Validators.FeedbackValidators;
public class AddFeedbackValidator : AbstractValidator<AddFeedbackParams>
{
    public AddFeedbackValidator()
    {
        RuleFor(x => x.TargetId)
            .GreaterThan(0)
            .WithMessage(string.Format(ValidationMessages.InvalidInput, "Đối tượng"));

        RuleFor(x => x.TargetType)
            .IsInEnum()
            .WithMessage(string.Format(ValidationMessages.InvalidInput, "Loại đối tượng"));

        RuleFor(x => x.Rating)
            .InclusiveBetween(1, 5)
            .WithMessage(string.Format(ValidationMessages.InvalidInput, "Số sao"));

        RuleFor(x => x.Content)
            .NotEmpty()
            .WithMessage(string.Format(ValidationMessages.RequiredField, "Nội dung"))
            .MaximumLength(ValidationLengths.Fields.Content)
            .WithMessage(string.Format(ValidationMessages.MaxLengthExceeded, "Nội dung", ValidationLengths.Fields.Content));
    }
}
