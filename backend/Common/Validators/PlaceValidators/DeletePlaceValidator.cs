using Common.DTOs.PlaceDtos;
using FluentValidation;
using Utility.Constants;

namespace Common.Validators.PlaceValidators;
public class DeletePlaceValidator : AbstractValidator<DeletePlaceParams>
{
    public DeletePlaceValidator()
    {
        RuleFor(x => x.Id)
          .NotEmpty().WithMessage(string.Format(ValidationMessages.RequiredField, "Đối tượng"));
    }
}
