using Common.DTOs.FriendshipDtos;
using FluentValidation;
using Utility.Constants;

namespace Common.Validators.FriendshipValidators;
public class FriendRequestValidator : AbstractValidator<FriendRequestParams>
{
    public FriendRequestValidator()
    {
        RuleFor(x => x.OtherUserId)
         .NotEmpty().WithMessage(string.Format(ValidationMessages.RequiredField, "Đối tượng"))
          .GreaterThan(1).WithMessage(string.Format(ValidationMessages.InvalidInput, "Đối tượng"));
    }
}
