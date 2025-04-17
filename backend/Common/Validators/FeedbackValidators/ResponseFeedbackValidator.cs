using Common.DTOs.FeedbackDtos;
using FluentValidation;
using Utility.Constants;

namespace Common.Validators.FeedbackValidators;
public class ResponseFeedbackValidator : AbstractValidator<ResponseFeedbackParams>
{
    public ResponseFeedbackValidator()
    {
        RuleFor(x => x.Id)
           .GreaterThan(0)
           .WithMessage(string.Format(ValidationMessages.InvalidInput, "Id"));

        RuleFor(x => x.Message)
            .NotEmpty()
            .WithMessage(string.Format(ValidationMessages.RequiredField, "Message"))
            .MaximumLength(ValidationLengths.Fields.Content)
            .WithMessage(string.Format(ValidationMessages.MaxLengthExceeded, "Message", ValidationLengths.Fields.Content));
    }

}
