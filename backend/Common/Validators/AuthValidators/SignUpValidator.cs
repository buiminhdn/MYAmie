using Common.DTOs.AuthDtos;
using FluentValidation;
using Utility.Constants;

namespace Common.Validators.AuthValidators;
public class SignUpValidator : AbstractValidator<SignUpParams>
{
    public SignUpValidator()
    {
        RuleFor(x => x.LastName)
             .NotEmpty().WithMessage(string.Format(ValidationMessages.RequiredField, "Họ"));

        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage(string.Format(ValidationMessages.RequiredField, "Tên"));

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage(string.Format(ValidationMessages.RequiredField, "Email"))
            .EmailAddress().WithMessage(string.Format(ValidationMessages.InvalidInput, "Email"));

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage(string.Format(ValidationMessages.RequiredField, "Mật khẩu"))
            .MinimumLength(6).WithMessage(string.Format(ValidationMessages.MinLengthRequired, "Mật khẩu", 6));
    }
}
