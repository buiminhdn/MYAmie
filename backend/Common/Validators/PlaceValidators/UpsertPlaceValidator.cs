using Common.DTOs.PlaceDtos;
using FluentValidation;
using Utility.Constants;

namespace Common.Validators.PlaceValidators;
public class UpsertPlaceValidator : AbstractValidator<UpsertPlaceParams>
{
    public UpsertPlaceValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage(string.Format(ValidationMessages.RequiredField, "Tên"))
            .MaximumLength(ValidationLengths.Fields.LongName)
            .WithMessage(string.Format(ValidationMessages.MaxLengthExceeded, "Tên", ValidationLengths.Fields.LongName));

        RuleFor(x => x.ShortDescription)
            .MaximumLength(ValidationLengths.MaxLengthDefault)
            .WithMessage(string.Format(ValidationMessages.MaxLengthExceeded, "Mô tả ngắn", ValidationLengths.MaxLengthDefault));

        RuleFor(x => x.CityId)
            .NotEmpty().WithMessage(string.Format(ValidationMessages.RequiredField, "Thành phố"))
            .GreaterThan(0).WithMessage(string.Format(ValidationMessages.InvalidInput, "Thành phố"));

        RuleFor(x => x.CategoryIds)
            .NotEmpty().WithMessage(string.Format(ValidationMessages.RequiredField, "Danh mục"))
            .Must(x => x.Count > 0).Must(x => x.Count <= 3).WithMessage(string.Format(ValidationMessages.CategoryCountInvalid, 1, 3));

        RuleFor(x => x.Address)
            .NotEmpty().WithMessage(string.Format(ValidationMessages.RequiredField, "Địa chỉ"))
            .MaximumLength(ValidationLengths.Fields.Address)
            .WithMessage(string.Format(ValidationMessages.MaxLengthExceeded, "Địa chỉ", ValidationLengths.Fields.Address));
    }
}
