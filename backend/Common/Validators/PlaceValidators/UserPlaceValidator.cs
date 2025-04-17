using Common.DTOs.PlaceDtos;
using FluentValidation;
using Utility.Constants;

namespace Common.Validators.PlaceValidators;
public class UserPlaceValidator : AbstractValidator<UserPlaceParams>
{
    public UserPlaceValidator()
    {
        RuleFor(x => x.UserId)
          .NotEmpty().WithMessage(string.Format(ValidationMessages.RequiredField, "Id"));
    }
}
