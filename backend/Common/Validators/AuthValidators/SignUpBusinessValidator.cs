using Common.DTOs.AuthDtos;
using FluentValidation;
using Utility.Constants;

namespace Common.Validators.AuthValidators;
public class SignUpBusinessValidator : AbstractValidator<SignUpBusinessParams>
{
    public SignUpBusinessValidator()
    {
        RuleFor(x => x.Email)
           .NotEmpty().WithMessage(string.Format(ValidationMessages.RequiredField, "Email"))
           .EmailAddress().WithMessage(string.Format(ValidationMessages.InvalidInput, "Email"));

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage(string.Format(ValidationMessages.RequiredField, "Mật khẩu"))
            .MinimumLength(6).WithMessage(string.Format(ValidationMessages.MinLengthRequired, "Mật khẩu", 6));

        RuleFor(x => x.ShortDescription)
          .MaximumLength(ValidationLengths.MaxLengthDefault)
          .WithMessage(string.Format(ValidationMessages.MaxLengthExceeded, "Mô tả ngắn", ValidationLengths.MaxLengthDefault));

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage(string.Format(ValidationMessages.RequiredField, "Tên"))
            .MaximumLength(ValidationLengths.Fields.Name)
            .WithMessage(string.Format(ValidationMessages.MaxLengthExceeded, "Tên", ValidationLengths.Fields.Name));

        RuleFor(x => x.CityId)
            .NotEmpty().WithMessage(string.Format(ValidationMessages.RequiredField, "Thành phố"))
            .GreaterThan(0).WithMessage(string.Format(ValidationMessages.InvalidInput, "Thành phố"));

        RuleFor(x => x.CategoryIds)
            .NotEmpty().WithMessage(string.Format(ValidationMessages.RequiredField, "Danh mục"))
            .Must(x => x.Count > 0).Must(x => x.Count <= 3).WithMessage(string.Format(ValidationMessages.CategoryCountInvalid, 1, 3));
    }
}
