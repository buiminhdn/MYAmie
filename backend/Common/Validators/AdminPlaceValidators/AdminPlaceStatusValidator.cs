using Common.DTOs.AdminPlaceDtos;
using FluentValidation;
using Utility.Constants;

namespace Common.Validators.AdminPlaceValidators;
public class AdminPlaceStatusValidator : AbstractValidator<AdminPlaceStatusParams>
{
    public AdminPlaceStatusValidator()
    {
        RuleFor(x => x.PlaceId)
            .GreaterThan(0)
            .WithMessage(string.Format(ValidationMessages.InvalidInput, "Id"));

        RuleFor(x => x.Status)
            .IsInEnum()
            .WithMessage(string.Format(ValidationMessages.InvalidInput, "Trạng thái"));
    }
}