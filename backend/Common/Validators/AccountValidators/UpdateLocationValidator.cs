using Common.DTOs.AccountDtos;
using FluentValidation;
using Utility.Constants;

namespace Common.Validators.AccountValidators;
public class UpdateLocationValidator : AbstractValidator<UpdateLocationParams>
{
    public UpdateLocationValidator()
    {
        RuleFor(x => x.Latitude)
            .NotEmpty().WithMessage(string.Format(ValidationMessages.RequiredField, "Toạ độ"));

        RuleFor(x => x.Longitude)
          .NotEmpty().WithMessage(string.Format(ValidationMessages.RequiredField, "Toạ độ"));
    }
}
