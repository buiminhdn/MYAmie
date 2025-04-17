using Common.DTOs.AccountDtos;
using FluentValidation;
using Models.Core;
using Utility.Constants;

namespace Common.Validators.AccountValidators;
public class UpdateProfileValidator : AbstractValidator<UpdateProfileParams>
{
    public UpdateProfileValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty()
            .WithMessage(string.Format(ValidationMessages.RequiredField, "Họ"))
            .MaximumLength(ValidationLengths.Fields.Name)
            .WithMessage(string.Format(ValidationMessages.MaxLengthExceeded, "Họ", ValidationLengths.Fields.Name));

        RuleFor(x => x.LastName)
            .NotEmpty()
            .WithMessage(string.Format(ValidationMessages.RequiredField, "Tên"))
            .MaximumLength(ValidationLengths.Fields.Name)
            .WithMessage(string.Format(ValidationMessages.MaxLengthExceeded, "Tên", ValidationLengths.Fields.Name));

        RuleFor(x => x.ShortDescription)
            .MaximumLength(ValidationLengths.MaxLengthDefault)
            .WithMessage(string.Format(ValidationMessages.MaxLengthExceeded, "Mô tả ngắn", ValidationLengths.MaxLengthDefault));

        RuleForEach(x => x.Characteristics)
           .MaximumLength(ValidationLengths.Fields.Characteristic)
           .WithMessage(string.Format(ValidationMessages.MaxLengthExceeded, "Tính cách", ValidationLengths.Fields.Characteristic));

        RuleFor(x => x.CurrentUserRole)
            .Equal(Role.USER)
            .WithMessage(ValidationMessages.Forbidden);
    }
}
