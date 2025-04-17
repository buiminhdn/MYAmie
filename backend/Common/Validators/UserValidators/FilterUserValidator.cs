using Common.DTOs.UserDtos;
using FluentValidation;
using Utility.Constants;

namespace Common.Validators.UserValidators;
public class FilterUserValidator : AbstractValidator<FilterUserParams>
{
    public FilterUserValidator()
    {
        RuleFor(x => x.Latitude)
            .NotEmpty().WithMessage(string.Format(ValidationMessages.RequiredField, "Toạ độ"));

        RuleFor(x => x.Longitude)
            .NotEmpty().WithMessage(string.Format(ValidationMessages.RequiredField, "Toạ độ"));
    }
}
