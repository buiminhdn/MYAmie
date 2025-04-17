using Common.DTOs.AccountDtos;
using FluentValidation;
using Utility.Constants;

namespace Common.Validators.AccountValidators;
public class UpdateAvatarOrCoverValidator : AbstractValidator<UpdateAvatarOrCoverParams>
{
    public UpdateAvatarOrCoverValidator()
    {
        RuleFor(x => x.Type)
           .Must(type => type == ImageTypeParam.Avatar || type == ImageTypeParam.Cover)
           .WithMessage(string.Format(ValidationMessages.InvalidInput, "Loại ảnh"));
    }
}
