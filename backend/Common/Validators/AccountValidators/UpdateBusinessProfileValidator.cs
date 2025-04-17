using Common.DTOs.AccountDtos;
using FluentValidation;
using Models.Core;
using Utility.Constants;

namespace Common.Validators.AccountValidators;
public class UpdateBusinessProfileValidator : AbstractValidator<UpdateBusinessProfileParams>
{
    public UpdateBusinessProfileValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage(string.Format(ValidationMessages.RequiredField, "Tên"))
            .MaximumLength(ValidationLengths.Fields.Name)
            .WithMessage(string.Format(ValidationMessages.MaxLengthExceeded, "Tên", ValidationLengths.Fields.Name));

        RuleFor(x => x.ShortDescription)
            .MaximumLength(ValidationLengths.MaxLengthDefault)
            .WithMessage(string.Format(ValidationMessages.MaxLengthExceeded, "Mô tả ngắn", ValidationLengths.MaxLengthDefault));

        RuleFor(x => x.Address)
            .MaximumLength(ValidationLengths.Fields.Address)
            .WithMessage(string.Format(ValidationMessages.MaxLengthExceeded, "Địa chỉ", ValidationLengths.Fields.Address));

        RuleFor(x => x.Phone)
            .MaximumLength(ValidationLengths.Fields.Phone)
            .WithMessage(string.Format(ValidationMessages.MaxLengthExceeded, "Số điện thoại", ValidationLengths.Fields.Phone));

        RuleFor(x => x.OpenHour)
            .InclusiveBetween(0, 23)
            .WithMessage(string.Format(ValidationMessages.InvalidInput, "Giờ mở cửa"));

        RuleFor(x => x.CloseHour)
            .InclusiveBetween(0, 23)
            .GreaterThan(x => x.OpenHour)
            .WithMessage("Giờ đóng cửa phải lớn hơn giờ mở cửa và nằm trong khoảng từ 0 đến 23.");

        RuleFor(x => x.CurrentUserRole)
         .Equal(Role.BUSINESS)
         .WithMessage(ValidationMessages.Forbidden);
    }
}
