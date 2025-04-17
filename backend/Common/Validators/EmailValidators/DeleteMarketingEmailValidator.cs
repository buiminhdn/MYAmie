using Common.DTOs.EmailDtos;
using FluentValidation;
using Utility.Constants;

namespace Common.Validators.EmailValidators;
public class DeleteMarketingEmailValidator : AbstractValidator<DeleteMarketingEmailParams>
{
    public DeleteMarketingEmailValidator()
    {
        RuleFor(x => x.Id)
         .NotEmpty().WithMessage(string.Format(ValidationMessages.RequiredField, "Đối tượng"));
    }
}
