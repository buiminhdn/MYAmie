using Common.DTOs.ChatDtos;
using FluentValidation;
using Utility.Constants;

namespace Common.Validators.ChatValidators;
public class PagedMessageValidator : AbstractValidator<PagedMessageParams>
{
    public PagedMessageValidator()
    {
        RuleFor(x => x.OtherUserId)
           .NotEmpty().WithMessage(string.Format(ValidationMessages.RequiredField, "Đối tượng"));
    }
}
