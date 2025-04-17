using Common.DTOs.EmailDtos;
using FluentValidation;
using Utility.Constants;

namespace Common.Validators.EmailValidators;
public class RequestVerifyValidator : AbstractValidator<RequestVerifyParams>
{
    public RequestVerifyValidator()
    {
        RuleFor(x => x.Email)
           .NotEmpty().WithMessage(string.Format(ValidationMessages.RequiredField, "Email"))
           .EmailAddress().WithMessage(string.Format(ValidationMessages.InvalidInput, "Email"));

        RuleFor(x => x.Type)
           .Must(type => type == VerificationTypeParam.AccountConfirmation || type == VerificationTypeParam.PasswordReset)
           .WithMessage(string.Format(ValidationMessages.InvalidInput, "Loại yêu cầu xác minh"));
    }
}
